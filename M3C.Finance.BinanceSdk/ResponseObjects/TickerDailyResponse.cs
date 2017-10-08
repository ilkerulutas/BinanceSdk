using Newtonsoft.Json;

namespace M3C.Finance.BinanceSdk.ResponseObjects
{
    public class TickerDailyResponse
    {
        public decimal PriceChange { get; set; }
        public decimal PriceChangePercent { get; set; }
        public decimal WeightedAveragePrice { get; set; }
        [JsonProperty("prevClosePrice")]
        public decimal PreviousClosePrice { get; set; }
        public decimal LastPrice { get; set; }
        public decimal BidPrice { get; set; }
        public decimal AskPrice { get; set; }
        public decimal OpenPrice { get; set; }
        public decimal HighPrice { get; set; }
        public decimal LowPrice { get; set; }
        public decimal Volume { get; set; }
        public long OpenTime { get; set; }
        public long CloseTime { get; set; }
        [JsonProperty("fristId")]
        public long FirstTradeId { get; set; }
        [JsonProperty("lastId")]
        public long LastTradeId { get; set; }
        public int Count { get; set; }

    }
}
