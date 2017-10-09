using Newtonsoft.Json;

namespace M3C.Finance.BinanceSdk.ResponseObjects
{
    public abstract class WebSocketMessageBase
    {
        [JsonProperty(PropertyName = "e")]
        public string EventType { get; set; }

        [JsonProperty(PropertyName = "E")]
        public long EventTime { get; set; }

        
    }

}
