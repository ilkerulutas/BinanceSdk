using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace M3C.Finance.BinanceSdk.ResponseObjects
{
    public class WsUserDataTradeUpdateMessage : WebSocketUserDataExecutionReportMessage
    {
        [JsonProperty(PropertyName = "l")]
        public decimal LastFilledTradeQuantity { get; set; }

        [JsonProperty(PropertyName = "z")]
        public decimal AccumulatedFilledTradeQuantity { get; set; }

        [JsonProperty(PropertyName = "L")]
        public decimal PriceOfLastFilledTrade { get; set; }

        [JsonProperty(PropertyName = "n")]
        public decimal Commission { get; set; }

        [JsonProperty(PropertyName = "N")]
        public string CommissionAsset { get; set; }

        [JsonProperty(PropertyName = "T")]
        public long TradeTime { get; set; }
    }
}
