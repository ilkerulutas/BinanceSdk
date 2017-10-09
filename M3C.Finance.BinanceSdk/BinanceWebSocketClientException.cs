using System;

namespace M3C.Finance.BinanceSdk
{
    public class BinanceWebSocketClientException : Exception
    {
        public BinanceWebSocketClientException(string message) : base(message)
        {
            
        }
    }
}
