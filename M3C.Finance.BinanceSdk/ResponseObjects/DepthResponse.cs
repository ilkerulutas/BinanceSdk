using System.Collections.Generic;

namespace M3C.Finance.BinanceSdk.ResponseObjects
{
    public class DepthResponse
    {
        public long LastUpdateId { get; set; }
        public List<OrderRecord> Bids { get; set; }
        public List<OrderRecord> Asks { get; set; }
            
    }
}
