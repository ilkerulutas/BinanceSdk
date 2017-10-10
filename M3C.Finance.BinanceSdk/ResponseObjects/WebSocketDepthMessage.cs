using System.Collections.Generic;
using Newtonsoft.Json;

namespace M3C.Finance.BinanceSdk.ResponseObjects
{
    public class WebSocketDepthMessage : WebSocketMessageBase
    {
        [JsonProperty(PropertyName = "s")]
        public string Symbol { get; set; }

        [JsonProperty(PropertyName = "u")]
        public long UpdateId { get; set; }

        [JsonProperty(PropertyName = "b")]
        public List<OrderRecord> BidDepthDelta { get; set; }

        [JsonProperty(PropertyName = "a")]
        public List<OrderRecord> AskDepthDelta { get; set; }
    }
}
