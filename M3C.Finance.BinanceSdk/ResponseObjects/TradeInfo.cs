using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace M3C.Finance.BinanceSdk.ResponseObjects
{
    public class TradeInfo
    {
        [JsonProperty(PropertyName = "id")]
        public long Id { get; set; }
        [JsonProperty(PropertyName = "price")]
        public decimal Price { get; set; }
        [JsonProperty(PropertyName = "qty")]
        public decimal Quantity { get; set; }
        [JsonProperty(PropertyName = "commission")]
        public decimal Commission { get; set; }
        [JsonProperty(PropertyName = "commissionAsset")]
        public string CommissionAsset { get; set; }
        [JsonProperty(PropertyName = "time")]
        public long Time { get; set; }
        [JsonProperty(PropertyName = "isBuyer")]
        public bool IsBuyer { get; set; }
        [JsonProperty(PropertyName = "isMaker")]
        public bool IsMaker { get; set; }
        [JsonProperty(PropertyName = "isBestMatch")]
        public bool IsBestMatch { get; set; }
    }
}
