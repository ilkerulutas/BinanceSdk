using System;
using System.Configuration;
using M3C.Finance.BinanceSdk.Enumerations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace M3C.Finance.BinanceSdk.Tests
{
    [TestClass]
    public class SignedRequestTests
    {
        private BinanceClient _client;

        [TestInitializeAttribute]
        public void Setup()
        {
            _client = new BinanceClient(ConfigurationManager.AppSettings["BinanceApiKey"], ConfigurationManager.AppSettings["BinanceApiSecret"]);
        }

        [TestMethod]
        public void TestAccountInfo()
        {
            var response = _client.GetAccountInfo();
        }

        [TestMethod]
        public void TestNewTestOrder()
        {
            var response = _client.NewOrder("ETHBTC",OrderSide.Sell,OrderType.Limit, TimeInForce.ImmediateOrCancel, 0.1m,1.0m,true);
        }

        [TestMethod]
        public void TestNewOrder()
        {
            var response = _client.NewOrder("NEOBTC", OrderSide.Sell, OrderType.Limit, TimeInForce.GoodUntilCanceled, 1m, 0.05m,false,"order1");
        }

        [TestMethod]
        public void TestQueryOrder()
        {
            var response = _client.QueryOrder("NEOBTC",5240943);
        }

        [TestMethod]
        public void TestOpenOrders()
        {
            var response = _client.CurrentOpenOrders("NEOBTC");
        }

        [TestMethod]
        public void TestListAllOrders()
        {
            var response = _client.ListAllOrders("NEOBTC");
        }

        [TestMethod]
        public void TestCancelOrder()
        {
            var response = _client.CancelOrder("NEOBTC",null,"order1");
        }

        [TestMethod]
        public void TestListMyTrades()
        {
            var response = _client.ListMyTrades("STRATBTC");
        }
    }
}
