using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace M3C.Finance.BinanceSdk.ResponseObjects
{
    public class AggregateTradeResponse : ResponseBase
    {
        public List<AggregateTradeResponseItem> List { get; set; }
    }
}
