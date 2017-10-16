/*******************************************************
 * Copyright (C) 2017 - Ilker Ulutas
 * 
 * This file is part of M3C.Finance.Binance
 * https://github.com/ilkerulutas/BinanceSdk
 * 
 * Released under the MIT License
 *******************************************************/

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
        private const int KeepAliveMilliseconds = 30000;
        private const string WebSocketBaseUrl = "wss://stream.binance.com:9443/ws/";
        private static readonly NLog.Logger logger = LogManager.GetCurrentClassLogger();
        private List<WebSocket> ActiveSockets;

        public delegate void WebSocketMessageHandler<T>(T messageContent) where T : WebSocketMessageBase;
        public delegate T ResponseParseHandler<T>(string input);

        public BinanceWebSocketClient()
        {
            ActiveSockets = new List<WebSocket>();
        }

        private string GetWsEndpoint(string method, string symbol)
        {
            var postfix = string.IsNullOrEmpty(method) ? string.Empty : $"@{method}";
            return $"{WebSocketBaseUrl}{symbol.ToLowerInvariant()}{postfix}";
        }

        public void ConnectDepthEndpoint(string symbol, WebSocketMessageHandler<WebSocketDepthMessage> messageHandler)
        {
            ConnectWebSocketEndpoint(GetWsEndpoint("depth", symbol), messageHandler, CustomJsonParsers.DepthMessageParser);
        }

        public void ConnectKlineEndpoint(string symbol, KlineInterval interval, WebSocketMessageHandler<WebSocketKlineMessage> messageHandler)
        {
            ConnectWebSocketEndpoint(GetWsEndpoint("kline_" + interval, symbol), messageHandler);
        }

        public void ConnectTradesEndpoint(string symbol, WebSocketMessageHandler<WebSocketTradesMessage> messageHandler)
        {
            ConnectWebSocketEndpoint(GetWsEndpoint("aggTrade", symbol), messageHandler);
        }

        public async Task<string> ConnectUserDataEndpoint(BinanceClient client, WebSocketMessageHandler<WsUserDataAccountUpdateMessage> accountUpdateHandler,
            WebSocketMessageHandler<WsUserDataOrderUpdateMessage> orderUpdateHandler, WebSocketMessageHandler<WsUserDataTradeUpdateMessage> tradeUpdateHandler)
        {
            var listenKey = await client.StartUserDataStream();
            var endpoint = GetWsEndpoint(string.Empty, listenKey);
            var ws = CreateNewWebSocket(endpoint, listenKey);

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
                        var executionType = (string)responseObject["x"];
                        if (executionType == ExecutionType.Trade)
                        {
                            tradeUpdateHandler(JsonConvert.DeserializeObject<WsUserDataTradeUpdateMessage>(e.Data));
                            return;
                        }
                        orderUpdateHandler(JsonConvert.DeserializeObject<WsUserDataOrderUpdateMessage>(e.Data));
                        return;
                    default:
                        throw new ApplicationException("Unexpected Event Type In Message");
                }
            };

            ws.Connect();
            ActiveSockets.Add(ws);

            var keepAliveTimer = new Timer(KeepAliveHandler, new KeepAliveContext
            {
                Client = client,
                ListenKey = listenKey
            }, KeepAliveMilliseconds, KeepAliveMilliseconds);

            return listenKey;
        }

        private static async void KeepAliveHandler(object context)
        {
            var ctx = (KeepAliveContext) context;
            logger.Debug("Making Keepalive Request for :" + ctx.ListenKey);
            await ctx.Client.KeepAliveUserDataStream(ctx.ListenKey);
        }

        public string ConnectUserDataEndpointSync(BinanceClient client,
            WebSocketMessageHandler<WsUserDataAccountUpdateMessage> accountUpdateHandler,
            WebSocketMessageHandler<WsUserDataOrderUpdateMessage> orderUpdateHandler,
            WebSocketMessageHandler<WsUserDataTradeUpdateMessage> tradeUpdateHandler)
            => ConnectUserDataEndpoint(client, accountUpdateHandler, orderUpdateHandler, tradeUpdateHandler).Result;

        private void ConnectWebSocketEndpoint<T>(string endpoint, WebSocketMessageHandler<T> messageHandler, ResponseParseHandler<T> customParseHandler = null) where T : WebSocketMessageBase
        {
            var ws = CreateNewWebSocket(endpoint);

            ws.OnMessage += (sender, e) =>
            {
                logger.Debug("Msg: " + e.Data);
                var eventData = customParseHandler == null ?
                    JsonConvert.DeserializeObject<T>(e.Data) :
                    customParseHandler(e.Data);
                messageHandler(eventData);
            };
            ws.Connect();
            ActiveSockets.Add(ws);
        }

        private BinanceWebSocket CreateNewWebSocket(string endpoint, string listenKey = null)
        {
            var ws = new BinanceWebSocket(endpoint, listenKey);

            ws.OnOpen += delegate { logger.Debug($"{endpoint} | Socket Connection Established ({ws.Id})"); };

            ws.OnClose += (sender, e) =>
            {
                ActiveSockets.Remove(ws);
                logger.Debug($"Socket Connection Closed! ({ws.Id})");
            };

            ws.OnError += (sender, e) =>
            {
                ActiveSockets.Remove(ws);
                logger.Debug("Msg: " + e.Message + " | " + e.Exception.Message);
            };
            return ws;
        }

        public void Dispose()
        {
            logger.Debug("Disposing WebSocket Client...");
            for (var i = ActiveSockets.Count - 1; i >= 0; i--)
            {
                ActiveSockets[i].Close();
            }
        }

        private class KeepAliveContext
        {
            public string ListenKey { get; set; }
            public BinanceClient Client { get; set; }
        }
    }
}
