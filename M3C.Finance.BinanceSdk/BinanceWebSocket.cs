using System;
using WebSocketSharp;

namespace M3C.Finance.BinanceSdk
{
    public class BinanceWebSocket : WebSocket
    {
        public string ListenKey { get; set; }
        public Guid Id { get; private set; }

        public BinanceWebSocket(string url,string listenKey = null, params string[] protocols) : base(url, protocols)
        {
            Id = Guid.NewGuid();
            ListenKey = listenKey;
        }
    }
}
