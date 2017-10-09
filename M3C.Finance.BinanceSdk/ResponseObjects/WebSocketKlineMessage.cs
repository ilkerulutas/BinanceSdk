using Newtonsoft.Json;

namespace M3C.Finance.BinanceSdk.ResponseObjects
{
    public class WebSocketKlineMessage : WebSocketMessageBase
    {
        [JsonProperty(PropertyName = "s")]
        public string Symbol { get; set; }

        [JsonProperty(PropertyName = "k")]
        public KLineEventItem KLineData { get; set; }
    }
}
