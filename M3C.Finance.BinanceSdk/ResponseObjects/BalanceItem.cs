using Newtonsoft.Json;

namespace M3C.Finance.BinanceSdk.ResponseObjects
{
    public class BalanceItem
    {
        [JsonProperty(PropertyName = "asset")]
        public string Asset { get; set; }

        [JsonProperty(PropertyName = "free")]
        public decimal Free { get; set; }

        [JsonProperty(PropertyName = "locked")]
        public decimal Locked { get; set; }
    }
}
