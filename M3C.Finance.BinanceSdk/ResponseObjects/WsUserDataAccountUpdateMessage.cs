using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace M3C.Finance.BinanceSdk.ResponseObjects
{
    public class WsUserDataAccountUpdateMessage : WebSocketMessageBase
    {
        [JsonProperty(PropertyName = "B")]
        public List<BalanceItemShortNames> BalanceInfo { get; set; }

        //Purpose of Following fields are not specified on API Documentation
        //Check for further updates: https://www.binance.com/restapipub.html#wss-endpoint
        [JsonProperty(PropertyName = "m")]
        public int Field1 { get; set; }
        [JsonProperty(PropertyName = "t")]
        public int Field2 { get; set; }
        [JsonProperty(PropertyName = "b")]
        public int Field3 { get; set; }
        [JsonProperty(PropertyName = "s")]
        public int Field4 { get; set; }
        [JsonProperty(PropertyName = "T")]
        public bool Field5 { get; set; }
        [JsonProperty(PropertyName = "W")]
        public bool Field6 { get; set; }
        [JsonProperty(PropertyName = "D")]
        public bool Field7 { get; set; }


    }
}
