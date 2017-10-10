using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using M3C.Finance.BinanceSdk.Enumerations;
using Newtonsoft.Json;

namespace M3C.Finance.BinanceSdk.ResponseObjects
{
    public class KLineEventItem
    {
        [JsonProperty(PropertyName = "t")]
        public long StartTimeOfBar { get; set; }

        [JsonProperty(PropertyName = "T")]
        public long EndTimeOfBar { get; set; }

        [JsonProperty(PropertyName = "s")]
        public string Symbol { get; set; }

        [JsonProperty(PropertyName = "i")]
        public KlineInterval Interval { get; set; }

        [JsonProperty(PropertyName = "f")]
        public long FirstTradeId { get; set; }

        [JsonProperty(PropertyName = "L")]
        public long LastTradeId { get; set; }

        [JsonProperty(PropertyName = "o")]
        public decimal Open { get; set; }

        [JsonProperty(PropertyName = "c")]
        public decimal Close { get; set; }

        [JsonProperty(PropertyName = "h")]
        public decimal High { get; set; }

        [JsonProperty(PropertyName = "l")]
        public decimal Low { get; set; }

        [JsonProperty(PropertyName = "v")]
        public decimal Volume { get; set; }

        [JsonProperty(PropertyName = "n")]
        public int NumberOfTrades { get; set; }

        [JsonProperty(PropertyName = "x")]
        public bool IsThisBarFinal { get; set; }

        [JsonProperty(PropertyName = "q")]
        public decimal QuoteVolume { get; set; }

        [JsonProperty(PropertyName = "V")]
        public decimal VolumeOfActiveBuy { get; set; }

        [JsonProperty(PropertyName = "Q")]
        public decimal QuoteVolumeOfActiveBuy { get; set; }

    }
}
