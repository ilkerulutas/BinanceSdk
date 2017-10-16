using System;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using M3C.Finance.BinanceSdk;
using M3C.Finance.BinanceSdk.Enumerations;
using M3C.Finance.BinanceSdk.ResponseObjects;

namespace M3C.Finance.Binance.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            //Since you dont need to provide any API keys for Public Endpoints
            //You may init client with an empty constructor
            var publicRestClient = new BinanceClient();

            //By default ALL Api methods return Task<T> so you can use them asyncronously.
            //For convenience Sync versions of all methods are also provided for developers who may prefer not to use async results.
            var timeResponseAsync = publicRestClient.Time();
            var syncResponse = publicRestClient.TimeSync();

            PublicEndpointExamples();
            SignedEndpointExamples();
            WebSocketExamples();

        }

        static async Task<TimeResponse> GetTimeAsyncExample(BinanceClient restClient)
        {
            return await restClient.Time();
        }

        static void PublicEndpointExamples()
        {
            //Since you dont need to provide any API keys for Public Endpoints
            //You may init client with an empty constructor
            var publicRestClient = new BinanceClient();

            //Gets the orderbook for the given symbol. 
            //You also have the option to limit the number of resulting records .
            var depthResult = publicRestClient.Depth("NEOBTC").Result;

            //Get only the first 10 Orderbook records
            var limitedDepthResult = publicRestClient.Depth("NEOBTC", 10).Result;
            Console.WriteLine($"First Ask Price: {depthResult.Asks[0].Price}, First Ask Quantity: {depthResult.Asks[0].Quantity}");
            Console.WriteLine($"NoLimit Record Count: {depthResult.Asks.Count}, Limited Record Count: {limitedDepthResult.Asks.Count}");

            //For gettings Compressed/Aggregate trades list
            var aggregateTrades = publicRestClient.AggregateTrades("NEOBTC").Result;
            Console.WriteLine($"AggTradeId: {aggregateTrades[0].AggregateTradeId} Price: {aggregateTrades[0].Price}  Quantity: {aggregateTrades[0].Quantity}  WasMaker: {aggregateTrades[0].WasBuyerTheMaker}");

            //Get KLines/Candlestick data for given Trade Pair
            var kLines = publicRestClient.KLines("NEOBTC", KlineInterval.Month1).Result;
            foreach (var item in kLines)
            {
                Console.WriteLine($"# of Trades: {item.NumberOfTrades} Close: {item.Close} Volume: {item.Volume}");
            }

            //Get Daily Ticker for given Trade Pair
            var dailyTicker = publicRestClient.TickerDaily("NEOBTC").Result;
            Console.WriteLine($"Ask: {dailyTicker.AskPrice} Bid: {dailyTicker.BidPrice} Change: {dailyTicker.PriceChange}");

            //Get All Ticker Information
            var allPricesTicker = publicRestClient.TickerAllPrices().Result;
            foreach (var item in allPricesTicker)
            {
                Console.WriteLine($"{item.Symbol} : {item.Price}");
            }

            //Get AllBookTicker information with price and quantity data
            var allBookTickers = publicRestClient.AllBookTickers().Result;
            foreach (var item in allBookTickers)
            {
                Console.WriteLine($"{item.Symbol} : AskPrice/Quantity {item.AskPrice} / {item.AskQuantity} BidPrice/Quantity {item.BidPrice} / {item.BidQuantity}");
            }
        }

        static void SignedEndpointExamples()
        {
            //You need to init client with Api Keys for Signed Endpoint requests
            var restClient = new BinanceClient(ConfigurationManager.AppSettings["BinanceApiKey"], ConfigurationManager.AppSettings["BinanceApiSecret"]);

            //Get Account Info and Current Balances
            var accountInfo = restClient.GetAccountInfo(true).Result;
            Console.WriteLine($"Maker/Taker Commission: {accountInfo.MakerCommission}/{accountInfo.TakerCommission}");
            foreach (var balanceItem in accountInfo.Balances)
            {
                Console.WriteLine($"{balanceItem.Asset} Total Amount: {balanceItem.Free + balanceItem.Locked}");
            }

            //Initiates an order. 
            //Ensure that isTestOrder parameter is set to false for real order requests (By default it is false).
            //ClientOrderId is optional. It is useful if you might need follow up requests like cancel etc. If you dont set BinanceApi assigns one for the order.
            var orderResult = restClient.NewOrder("NEOBTC", OrderSide.Sell, OrderType.Limit, TimeInForce.GoodUntilCanceled, 100m, 0.05m, true, "order1").Result;
            Console.WriteLine($"Accepted with Id: {orderResult.OrderId} @ Time: {orderResult.TransactionTime}");

            //Query the given order result.
            var orderQueryResult = restClient.QueryOrder("NEOBTC", null, "order1").Result;
            Console.WriteLine($"Status: {orderQueryResult.Status} Executed Qty: {orderQueryResult.ExecutedQuantity} Orig Qty: {orderQueryResult.OriginalQuantity}");
            
            //List Current Open Orders for given Trade Pair
            var openOrders = restClient.CurrentOpenOrders("NEOBTC").Result;
            foreach (var openOrder in openOrders)
            {
                Console.WriteLine($"{openOrder.OrderId}: {openOrder.Status}");
                if (openOrder.Status == OrderStatus.PartiallyFilled)
                {
                    Console.WriteLine("This order has been partially filled");
                }
            }

            //List All Orders including closed/fulfilled etc...
            var allOrders = restClient.ListAllOrders("NEOBTC").Result;
            foreach (var orderInfo in allOrders)
            {
                Console.WriteLine($"{orderInfo.OrderId}: {orderInfo.Status} {orderInfo.OrderType}");
            }

            //Cancels an open order by using either orderId or clientOrderId
            var cancelOrderResult = restClient.CancelOrder("NEOBTC", null, "order1").Result;
            Console.WriteLine($"{cancelOrderResult.OrderId} ({cancelOrderResult.ClientOrderId}) : {cancelOrderResult.Symbol}");

            //List Trades
            var trades = restClient.ListMyTrades("NEOBTC").Result;
            foreach (var tradeInfo in trades)
            {
                Console.WriteLine($"{tradeInfo.Time} Qty: {tradeInfo.Quantity} Commission: {tradeInfo.Commission} {tradeInfo.CommissionAsset}");
            }
        }

        static void WebSocketExamples()
        {
            //Listen For Depth Messages
            using (var client = new BinanceWebSocketClient())
            {
                client.ConnectDepthEndpoint("ethbtc", depthMsg 
                    => Console.WriteLine($"{depthMsg.EventTime} {depthMsg.Symbol} {depthMsg.AskDepthDelta.Sum(b=>b.Quantity)}"));
                Thread.Sleep(60000);
            }

            //Listen For KLine Messages
            using (var client = new BinanceWebSocketClient())
            {
                client.ConnectKlineEndpoint("ethbtc", KlineInterval.Minute1, klineMsg 
                    => Console.WriteLine($"{klineMsg.Symbol} {klineMsg.KLineData.NumberOfTrades}"));
                Thread.Sleep(60000);
            }

            //Listen For Trades Messages
            using (var client = new BinanceWebSocketClient())
            {
                client.ConnectTradesEndpoint("ethbtc", tradesMsg 
                    => Console.WriteLine($"{tradesMsg.Symbol} {tradesMsg.Price} {tradesMsg.Quantity}") );
                Thread.Sleep(60000);
            }

            //Listen User Data Endpoint
            //Return ListenKey if you need to explicitly call CloseUserDataStream, KeepAliveUserDataStream from the rest client
            //Automatically makes keepalive request every 30 seconds
            var restClient = new BinanceClient(ConfigurationManager.AppSettings["BinanceApiKey"], ConfigurationManager.AppSettings["BinanceApiSecret"]);
            using (var client = new BinanceWebSocketClient())
            {
                var listenKey = client.ConnectUserDataEndpoint(restClient,
                    accountMessage => Console.WriteLine("UserData Received! " + accountMessage.EventTime),
                    orderMessage => Console.WriteLine("Order Message Received! " + orderMessage.EventTime),
                    tradeMessage => Console.WriteLine("Trade Message Received! " + tradeMessage.EventTime)
                ).Result;
                Thread.Sleep(300000);
            }
        }
    }
}
