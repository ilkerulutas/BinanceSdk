# BinanceSdk
Binance Rest Api and Web Socket Integrations for c#

Usage examples for each function can be found in the test project. 
You need to enter your own Api Credentials in the App.config file of the test project if you plan to run test functions.

# Usage

## REST CLIENT

```csharp
var client = new BinanceClient(["BinanceApiKey"], ["BinanceApiSecret"]);
var response = client.GetAccountInfo();
```

### WEBSOCKET CLIENT

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

* For User Data Endpoint you need to define three separate handlers since three types of message can be received through the socket connection.

```csharp
var client = new BinanceClient(["BinanceApiKey"], ["BinanceApiSecret"]);
using (var wsClient = new BinanceWebSocketClient())
{
    wsClient.ConnectUserDataEndpoint(client,
       accountMessage =>  logger.Debug("AccountUpdate Received! " + accountMessage.EventTime),
       orderMessage => logger.Debug("Order Message Received! " + orderMessage.EventTime),
       tradeMessage => logger.Debug("Trade Message Received! " + tradeMessage.EventTime)
    );
    Thread.Sleep(300000);
}
```
