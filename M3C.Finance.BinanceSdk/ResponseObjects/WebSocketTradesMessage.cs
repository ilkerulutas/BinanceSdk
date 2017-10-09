using Newtonsoft.Json;

namespace M3C.Finance.BinanceSdk.ResponseObjects
{
    public class WebSocketTradesMessage : WebSocketMessageBase
    {
        [JsonProperty(PropertyName = "s")]
        public string Symbol { get; set; }

        [JsonProperty(PropertyName = "a")]
        public long AggregatedTradeId { get; set; }

        [JsonProperty(PropertyName = "p")]
        public decimal Price { get; set; }

        [JsonProperty(PropertyName = "q")]
        public decimal Quantity { get; set; }

        [JsonProperty(PropertyName = "f")]
        public long FirstBreakdownTradeId { get; set; }

        [JsonProperty(PropertyName = "l")]
        public long LastBreakdownTradeId { get; set; }

        [JsonProperty(PropertyName = "T")]
        public long TradeTime { get; set; }

        [JsonProperty(PropertyName = "m")]
        public bool IsBuyerAMaker { get; set; }
    }
}
