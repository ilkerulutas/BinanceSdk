using M3C.Finance.BinanceSdk.Enumerations;
using Newtonsoft.Json;

namespace M3C.Finance.BinanceSdk.ResponseObjects
{
    public class WebSocketUserDataExecutionReportMessage : WebSocketMessageBase
    {
        [JsonProperty(PropertyName = "s")]
        public string Symbol { get; set; }

        [JsonProperty(PropertyName = "c")]
        public string NewClientOrderId { get; set; }

        [JsonProperty(PropertyName = "S")]
        public OrderSide OrderSide { get; set; }

        [JsonProperty(PropertyName = "o")]
        public OrderType OrderType { get; set; }

        [JsonProperty(PropertyName = "f")]
        public TimeInForce TimeInForce { get; set; }

        [JsonProperty(PropertyName = "q")]
        public decimal OriginalQuantity { get; set; }

        [JsonProperty(PropertyName = "p")]
        public decimal Price { get; set; }

        [JsonProperty(PropertyName = "x")]
        public ExecutionType ExecutionType { get; set; }

        [JsonProperty(PropertyName = "X")]
        public OrderStatus OrderStatus { get; set; }

        [JsonProperty(PropertyName = "r")]
        public OrderRejectReason OrderRejectReason { get; set; }

        [JsonProperty(PropertyName = "i")]
        public long OrderId { get; set; }
        
        [JsonProperty(PropertyName = "t")]
        public long TradeId { get; set; }

        [JsonProperty(PropertyName = "m")]
        public bool IsBuyerMaker { get; set; }


        //Purpose of Following fields are not specified on API Documentation
        //Check for further updates: https://www.binance.com/restapipub.html#wss-endpoint
        //[JsonProperty(PropertyName="P")]
        //public decimal Field1 { get; set; }
        //[JsonProperty(PropertyName = "F")]
        //public decimal Field2 { get; set; }
        //[JsonProperty(PropertyName = "g")]
        //public int Field3 { get; set; }
        //[JsonProperty(PropertyName = "C")]
        //public string Field4 { get; set; }
        
    }
}
