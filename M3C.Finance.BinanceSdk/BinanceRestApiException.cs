using System;

namespace M3C.Finance.BinanceSdk
{
    public class BinanceRestApiException : Exception
    {
        public BinanceRestApiException(int errorCode, string message) :base(message)
        {
            ErrorCode = errorCode;
        }
        public int ErrorCode { get; }
        public override string ToString()
        {
            return $"Code: {ErrorCode} | Message: {Message}";
        }
    }
}
