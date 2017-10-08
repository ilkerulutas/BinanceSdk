using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace M3C.Finance.BinanceSdk.ResponseObjects
{
    public class NewOrderResponse
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }
        [JsonProperty("orderId")]
        public int OrderId { get; set; }
        [JsonProperty("clientOrderId")]
        public string ClientOrderId { get; set; }
        [JsonProperty("transactTime")]
        public long TransactionTime { get; set; }
    }
}
