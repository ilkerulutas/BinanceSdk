using Newtonsoft.Json;

namespace M3C.Finance.BinanceSdk.ResponseObjects
{
    public class WsUserDataOrderUpdateMessage : WebSocketMessageBase
    {
        [JsonProperty(PropertyName = "T")]
        public long OrderTime { get; set; }
    }
}
