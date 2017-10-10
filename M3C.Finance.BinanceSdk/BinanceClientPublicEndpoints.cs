using System.Collections.Generic;
using System.Net.Http;
using M3C.Finance.BinanceSdk.Enumerations;
using M3C.Finance.BinanceSdk.ResponseObjects;
using Newtonsoft.Json.Linq;

namespace M3C.Finance.BinanceSdk
{
    public partial class BinanceClient
    {
        public bool Ping()
        {
            var response = SendRequest<JObject>("ping",ApiVersion.Version1 ,ApiMethodType.None,  HttpMethod.Get);
            return response.Type == JTokenType.Object;
        }

        public TimeResponse Time()
        {
            return SendRequest<TimeResponse>("time", ApiVersion.Version1, ApiMethodType.None, HttpMethod.Get);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <param name="limit">Optional: Default 100, Max 100</param>
        public DepthResponse Depth(string symbol, int? limit = null)
        {
            var parameters = new Dictionary<string, string> {{"symbol", symbol}};
            if (limit.HasValue)
            {
                parameters.Add("limit",limit.Value.ToString());
            }
            return SendRequest("depth", ApiVersion.Version1, ApiMethodType.None, HttpMethod.Get, parameters,CustomJsonParsers.DepthResponseParser);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="fromId">ID to get aggregate trades from INCLUSIVE.</param>
        /// <param name="startTime">ID to get aggregate trades from INCLUSIVE.</param>
        /// <param name="endTime">Timestamp in ms to get aggregate trades until INCLUSIVE.</param>
        /// <param name="limit">Default 500; max 500.</param>
        public List<AggregateTradeResponseItem> AggregateTrades(string symbol, long? fromId = null, long? startTime = null, long? endTime = null, int? limit = null)
        {
            var parameters = new Dictionary<string, string> { { "symbol", symbol } };
            if (fromId.HasValue)
            {
                parameters.Add("fromId", fromId.Value.ToString());
            }
            if (startTime.HasValue)
            {
                parameters.Add("startTime", startTime.Value.ToString());
            }
            if (endTime.HasValue)
            {
                parameters.Add("endTime", endTime.Value.ToString());
            }
            if (limit.HasValue)
            {
                parameters.Add("limit", limit.Value.ToString());
            }
            return SendRequest<List<AggregateTradeResponseItem>>("aggTrades", ApiVersion.Version1, ApiMethodType.None, HttpMethod.Get, parameters);
        }


        public IEnumerable<KLinesResponseItem> KLines(string symbol, KlineInterval interval, int? limit = null, long? startTime = null,long? endTime = null)
        {
            var parameters = new Dictionary<string, string> { { "symbol", symbol }, {"interval",interval} };
            if (limit.HasValue)
            {
                parameters.Add("limit", limit.Value.ToString());
            }
            if (startTime.HasValue)
            {
                parameters.Add("startTime", startTime.Value.ToString());
            }
            if (endTime.HasValue)
            {
                parameters.Add("endTime", endTime.Value.ToString());
            }
            return SendRequest("klines", ApiVersion.Version1, ApiMethodType.None, HttpMethod.Get, parameters,CustomJsonParsers.KLineResponseParser);
        }

        public TickerDailyResponse TickerDaily(string symbol)
        {
            var parameters = new Dictionary<string, string> { { "symbol", symbol } };
            return SendRequest<TickerDailyResponse>("ticker/24hr", ApiVersion.Version1, ApiMethodType.None, HttpMethod.Get, parameters);
        }

        public IEnumerable<TickerSummary> TickerAllPrices()
        {
            return SendRequest<List<TickerSummary>>("ticker/allPrices", ApiVersion.Version1, ApiMethodType.None, HttpMethod.Get);
        }

        public IEnumerable<TickerDetail> AllBookTickers()
        {
            return SendRequest<List<TickerDetail>>("ticker/allBookTickers", ApiVersion.Version1, ApiMethodType.None, HttpMethod.Get);
        }


    }
}
