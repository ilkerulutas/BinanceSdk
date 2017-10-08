using System.Collections.Generic;

namespace M3C.Finance.BinanceSdk.ResponseObjects
{
    public class QueryMultipleOrdersResponse
    {
        public List<OrderInfo> Items { get; set; }
    }
}
