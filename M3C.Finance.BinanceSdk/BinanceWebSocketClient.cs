using System;
using System.Collections.Generic;
using System.Linq;
using M3C.Finance.BinanceSdk.Enumerations;
using M3C.Finance.BinanceSdk.ResponseObjects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using WebSocketSharp;

namespace M3C.Finance.BinanceSdk
{
    public class BinanceWebSocketClient : IDisposable
    {

        private const string WebSocketBaseUrl = "wss://stream.binance.com:9443/ws/";
        private static readonly NLog.Logger logger = LogManager.GetCurrentClassLogger();
        private Dictionary<Guid,WebSocket> ActiveSockets;

        public delegate void WebSocketMessageHandler<T>(T messageContent) where T : WebSocketMessageBase;
        public delegate T ResponseParseHandler<T>(string input);

        public BinanceWebSocketClient()
        {
            ActiveSockets = new Dictionary<Guid, WebSocket>();
        }

        private string GetWsEndpoint(string method, string symbol)
        {
            var postfix = string.IsNullOrEmpty(method) ? string.Empty : $"@{method}";
            return $"{WebSocketBaseUrl}{symbol.ToLowerInvariant()}{postfix}";
        }

        public void ConnectDepthEndpoint(string symbol, WebSocketMessageHandler<WebSocketDepthMessage> messageHandler)
        {
            ConnectWebSocketEndpoint(GetWsEndpoint("depth",symbol), messageHandler, CustomJsonParsers.DepthMessageParser);   
        }

        public void ConnectKlineEndpoint(string symbol, KlineInterval interval,WebSocketMessageHandler<WebSocketKlineMessage> messageHandler)
        {
            ConnectWebSocketEndpoint(GetWsEndpoint("kline_" + interval,symbol),messageHandler);
        }

        public void ConnectTradesEndpoint(string symbol, WebSocketMessageHandler<WebSocketTradesMessage> messageHandler)
        {
            ConnectWebSocketEndpoint(GetWsEndpoint("aggTrade", symbol), messageHandler);
        }

        public void ConnectUserDataEndpoint(BinanceClient client, WebSocketMessageHandler<WsUserDataAccountUpdateMessage> accountUpdateHandler, 
            WebSocketMessageHandler<WsUserDataOrderUpdateMessage> orderUpdateHandler, WebSocketMessageHandler<WsUserDataTradeUpdateMessage> tradeUpdateHandler)
        {
            var listenKey = client.StartUserDataStream();
            var endpoint = GetWsEndpoint(string.Empty, listenKey);

            var wsId = Guid.NewGuid();
            var ws = CreateNewWebSocket(endpoint, wsId);

            ws.OnMessage += (sender, e) =>
            {
                logger.Debug("Msg: " + e.Data);
                var responseObject = JObject.Parse(e.Data);
                var eventType = (string)responseObject["e"];

                switch (eventType)
                {
                    case "outboundAccountInfo":
                        accountUpdateHandler(JsonConvert.DeserializeObject<WsUserDataAccountUpdateMessage>(e.Data));
                        return;
                    case "executionReport":
                        var executionType = (string) responseObject["x"];
                        if (executionType == ExecutionType.Trade)
                        {
                            tradeUpdateHandler(JsonConvert.DeserializeObject<WsUserDataTradeUpdateMessage>(e.Data));
                            return;
                        }
                        orderUpdateHandler(JsonConvert.DeserializeObject<WsUserDataOrderUpdateMessage>(e.Data));
                        return;
                    default:
                        throw new BinanceWebSocketClientException("Unexpected Event Type In Message");
                }
            };

            ws.Connect();
            ActiveSockets.Add(wsId, ws);
        }

        private void ConnectWebSocketEndpoint<T>(string endpoint, WebSocketMessageHandler<T> messageHandler, ResponseParseHandler<T> customParseHandler = null)  where  T : WebSocketMessageBase
        {
            var wsId = Guid.NewGuid();
            var ws = CreateNewWebSocket(endpoint, wsId);

            ws.OnMessage += (sender, e) =>
            {
                logger.Debug("Msg: " + e.Data);
                var eventData = customParseHandler == null ?
                    JsonConvert.DeserializeObject<T>(e.Data) :
                    customParseHandler(e.Data);
                messageHandler(eventData);
            };
            ws.Connect();
            ActiveSockets.Add(wsId, ws);
        }

        private WebSocket CreateNewWebSocket(string endpoint, Guid wsId)
        {
            var ws = new WebSocket(endpoint);

            ws.OnOpen += delegate { logger.Debug($"{endpoint} | Socket Connection Established ({wsId})"); };

            ws.OnClose += (sender, e) =>
            {
                ActiveSockets.Remove(wsId);
                logger.Debug($"Socket Connection Closed! ({wsId})");
            };

            ws.OnError += (sender, e) =>
            {
                ActiveSockets.Remove(wsId);
                logger.Debug("Msg: " + e.Message + " | " + e.Exception.Message);
            };
            return ws;
        }

        public void Dispose()
        {
            logger.Debug("Disposing WebSocket Client...");
            var keys = ActiveSockets.Keys.ToArray();
            foreach (var t in keys)
            {
                ActiveSockets[t].Close();
            }
        }
    }
}
