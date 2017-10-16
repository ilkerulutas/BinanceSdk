# BinanceSdk
Binance Rest Api and Web Socket Integrations for c#

* Provides both Async and Sync Methods
* Added an Example Console App which contains all API calls.
* Usage examples for each function can alse be found in the test project. 
* You need to enter your own Api Credentials in the App.config file of the test/example projects if needed.

# Usage

Note: If you are having trouble with nuget managed libraries:
From Visual Studo go to Tools => NuGetPackage Manager => Package Manager Console and run command "Update-Package -reinstall" it will resolve libraries and update your solution accordingly.

## REST CLIENT

By default ALL Api methods return Task<T> so you can use them asyncronously.
For convenience Sync versions of all methods are also provided for developers who may prefer not to use async results.

```csharp
var timeResponseAsync = publicRestClient.Time();
var syncResponse = publicRestClient.TimeSync();
```

### Supported API Endpoints

#### Public Methods

```csharp
var publicRestClient = new BinanceClient();

//Gets the orderbook for the given symbol. 
var depthResult = publicRestClient.Depth("NEOBTC");

//Get only the first 10 Orderbook records
var limitedDepthResult = publicRestClient.Depth("NEOBTC", 10);

//For getting Compressed/Aggregate trades list
var aggregateTrades = publicRestClient.AggregateTrades("NEOBTC");

//Get KLines/Candlestick data for given Trade Pair
var kLines = publicRestClient.KLines("NEOBTC", KlineInterval.Month1);

//Get Daily Ticker for given Trade Pair
var dailyTicker = publicRestClient.TickerDaily("NEOBTC");

//Get All Ticker Information
var allPricesTicker = publicRestClient.TickerAllPrices();

//Get AllBookTicker information with price and quantity data
var allBookTickers = publicRestClient.AllBookTickers();
```

#### Signed Methods

```csharp
var restClient = new BinanceClient([APIKEY], [APISECRET]);

//Get Account Info and Current Balances
var accountInfo = restClient.GetAccountInfo(true);

//Initiates an order. 
//Ensure that isTestOrder parameter is set to false for real order requests (By default it is false).
//ClientOrderId is optional. It is useful if you might need follow up requests like cancel etc. If you dont set BinanceApi assigns one for the order.
var orderResult = restClient.NewOrder("NEOBTC", OrderSide.Sell, OrderType.Limit, TimeInForce.GoodUntilCanceled, 100m, 0.05m, true, "order1");

//Query the given order result.
var orderQueryResult = restClient.QueryOrder("NEOBTC", null, "order1");

 //List Current Open Orders for given Trade Pair
var openOrders = restClient.CurrentOpenOrders("NEOBTC");

//List All Orders
var allOrders = restClient.ListAllOrders("NEOBTC");

//Cancels an open order by using either orderId or clientOrderId
var cancelOrderResult = restClient.CancelOrder("NEOBTC", null, "order1");

//List Trades
var trades = restClient.ListMyTrades("NEOBTC");
```
            
## WEBSOCKET CLIENT

* You can use anonymous functions

```csharp
using (var wsClient = new BinanceWebSocketClient())
{
    wsClient.ConnectDepthEndpoint("ethbtc", depthMessage => logger.Debug(depthMessage.EventTime));
    Thread.Sleep(10000); //to keep socket open, you need to implement your own logic accordingly
}
```

* OR Explicitly define a handler function

```csharp
private void DepthMessageHandler(WebSocketDepthMessage message)
{
    logger.Debug(message.EventTime);
    ...
    ...
}
using (var wsClient = new BinanceWebSocketClient())
{
    wsClient.ConnectDepthEndpoint("ethbtc", DepthMessageHandler);
    Thread.Sleep(10000);
}
```

#### Depth Messages

```csharp
using (var client = new BinanceWebSocketClient())
{
    client.ConnectDepthEndpoint("ethbtc", depthMsg 
        => Console.WriteLine($"{depthMsg.EventTime} {depthMsg.Symbol} {depthMsg.AskDepthDelta.Sum(b=>b.Quantity)}"));
    Thread.Sleep(60000);
}
```

#### KLine Messages

```csharp
using (var client = new BinanceWebSocketClient())
{
    client.ConnectKlineEndpoint("ethbtc", KlineInterval.Minute1, klineMsg 
        => Console.WriteLine($"{klineMsg.Symbol} {klineMsg.KLineData.NumberOfTrades}"));
    Thread.Sleep(60000);
}
```

#### Trades Messages

```csharp
using (var client = new BinanceWebSocketClient())
{
    client.ConnectTradesEndpoint("ethbtc", tradesMsg 
        => Console.WriteLine($"{tradesMsg.Symbol} {tradesMsg.Price} {tradesMsg.Quantity}") );
    Thread.Sleep(60000);
}
```

#### User Data Messages

```csharp
//Listen User Data Endpoint
//Return ListenKey if you need to explicitly call CloseUserDataStream, KeepAliveUserDataStream from the rest client
//Automatically makes keepalive request every 30 seconds
var restClient = new BinanceClient([APIKEY], [APISECRET]);

using (var client = new BinanceWebSocketClient())
{
    var listenKey = client.ConnectUserDataEndpoint(restClient,
        accountMessage => Console.WriteLine("UserData Received! " + accountMessage.EventTime),
        orderMessage => Console.WriteLine("Order Message Received! " + orderMessage.EventTime),
        tradeMessage => Console.WriteLine("Trade Message Received! " + tradeMessage.EventTime)
    ).Result;
    Thread.Sleep(300000);
}
```
