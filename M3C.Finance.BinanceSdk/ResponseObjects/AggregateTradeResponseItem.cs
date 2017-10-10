using Newtonsoft.Json;

namespace M3C.Finance.BinanceSdk.ResponseObjects
{
    public class AggregateTradeResponseItem
    {
        [JsonProperty("a")]
        public long AggregateTradeId { get; set; }

        [JsonProperty("p")]
        public decimal Price { get; set; }

        [JsonProperty("q")]
        public decimal Quantity { get; set; }

        [JsonProperty("f")]
        public long FirstTradeId { get; set; }

        [JsonProperty("l")]
        public long LastTradeId { get; set; }

        [JsonProperty("T")]
        public long Timestamp { get; set; }

        [JsonProperty("m")]
        public bool WasBuyerTheMaker { get; set; }

        [JsonProperty("M")]
        public bool WasTradeBestPriceMatch { get; set; }
    }
}
