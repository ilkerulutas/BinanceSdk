using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using M3C.Finance.BinanceSdk.Enumerations;
using M3C.Finance.BinanceSdk.ResponseObjects;
using Newtonsoft.Json.Linq;

namespace M3C.Finance.BinanceSdk
{
    public partial class BinanceClient
    {
        public async Task<bool> Ping()
        {
            var response = await SendRequest<JObject>("ping", ApiVersion.Version1, ApiMethodType.None, HttpMethod.Get);
            return response.Type == JTokenType.Object;
        }

        public bool PingSync() => Ping().Result;

        public async Task<TimeResponse> Time()
        {
            return await SendRequest<TimeResponse>("time", ApiVersion.Version1, ApiMethodType.None, HttpMethod.Get);
        }

        public TimeResponse TimeSync() => Time().Result;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <param name="limit">Optional: Default 100, Max 100</param>
        public async Task<DepthResponse> Depth(string symbol, int? limit = null)
        {
            var parameters = new Dictionary<string, string> { { "symbol", symbol } };
            if (limit.HasValue)
            {
                parameters.Add("limit", limit.Value.ToString());
            }
            return await SendRequest("depth", ApiVersion.Version1, ApiMethodType.None, HttpMethod.Get, parameters, CustomJsonParsers.DepthResponseParser);
        }

        public DepthResponse DepthSync(string symbol, int? limit = null) => Depth(symbol, limit).Result;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="fromId">ID to get aggregate trades from INCLUSIVE.</param>
        /// <param name="startTime">ID to get aggregate trades from INCLUSIVE.</param>
        /// <param name="endTime">Timestamp in ms to get aggregate trades until INCLUSIVE.</param>
        /// <param name="limit">Default 500; max 500.</param>
        public async Task<List<AggregateTradeResponseItem>> AggregateTrades(string symbol, long? fromId = null, long? startTime = null, long? endTime = null, int? limit = null)
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
            return await SendRequest<List<AggregateTradeResponseItem>>("aggTrades", ApiVersion.Version1, ApiMethodType.None, HttpMethod.Get, parameters);
        }

        public List<AggregateTradeResponseItem> AggregateTradesSync(string symbol, long? fromId = null, long? startTime = null, long? endTime = null, int? limit = null)
            => AggregateTrades(symbol, fromId, startTime, endTime, limit).Result;

        public async Task<IEnumerable<KLinesResponseItem>> KLines(string symbol, KlineInterval interval, int? limit = null, long? startTime = null, long? endTime = null)
        {
            var parameters = new Dictionary<string, string> { { "symbol", symbol }, { "interval", interval } };
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
            return await SendRequest("klines", ApiVersion.Version1, ApiMethodType.None, HttpMethod.Get, parameters, CustomJsonParsers.KLineResponseParser);
        }

        public IEnumerable<KLinesResponseItem> KLinesSync(string symbol, KlineInterval interval, int? limit = null, long? startTime = null, long? endTime = null)
            => KLines(symbol, interval, limit, startTime, endTime).Result;

        public async Task<TickerDailyResponse> TickerDaily(string symbol)
        {
            var parameters = new Dictionary<string, string> { { "symbol", symbol } };
            return await SendRequest<TickerDailyResponse>("ticker/24hr", ApiVersion.Version1, ApiMethodType.None, HttpMethod.Get, parameters);
        }

        public TickerDailyResponse TickerDailySync(string symbol) => TickerDaily(symbol).Result;

        public async Task<IEnumerable<TickerSummary>> TickerAllPrices()
        {
            return await SendRequest<List<TickerSummary>>("ticker/allPrices", ApiVersion.Version1, ApiMethodType.None, HttpMethod.Get);
        }

        public IEnumerable<TickerSummary> TickerAllPricesSync() => TickerAllPrices().Result;

        public async Task<IEnumerable<TickerDetail>> AllBookTickers()
        {
            return await SendRequest<List<TickerDetail>>("ticker/allBookTickers", ApiVersion.Version1, ApiMethodType.None, HttpMethod.Get);
        }

        public IEnumerable<TickerDetail> AllBookTickersSync() => AllBookTickers().Result;

    }
}
