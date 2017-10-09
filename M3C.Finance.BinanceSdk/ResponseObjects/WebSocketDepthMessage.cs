using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace M3C.Finance.BinanceSdk.ResponseObjects
{
    public class WebSocketDepthMessage : WebSocketMessageBase
    {
        //[JsonProperty(PropertyName = "T")]
        //public long TradeTime { get; set; }

        //[JsonProperty(PropertyName = "a")]
        //public long AggregatedTradeId { get; set; }

        //[JsonProperty(PropertyName = "f")]
        //public long FirstBreakdownTradeId { get; set; }

        //[JsonProperty(PropertyName = "l")]
        //public long LastBreakdownTradeId { get; set; }

        //[JsonProperty(PropertyName = "m")]
        //public bool IsBuyerAMaker { get; set; }

        //[JsonProperty(PropertyName = "p")]
        //public decimal Price { get; set; }

        //[JsonProperty(PropertyName = "q")]
        //public decimal Quantity { get; set; }

        [JsonProperty(PropertyName = "s")]
        public string Symbol { get; set; }

        [JsonProperty(PropertyName = "u")]
        public long UpdateId { get; set; }

        //[JsonProperty(PropertyName = "k")]
        //public KLineEventItem KLineEventItem { get; set; }

        [JsonProperty(PropertyName = "b")]
        public List<OrderRecord> BidDepthDelta { get; set; }

        [JsonProperty(PropertyName = "a")]
        public List<OrderRecord> AskDepthDelta { get; set; }
    }
}
