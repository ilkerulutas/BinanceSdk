using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M3C.Finance.BinanceSdk.ResponseObjects
{
    public class KLinesResponse
    {
        public List<KLinesResponseItem> Items { get; set; }
    }
}
