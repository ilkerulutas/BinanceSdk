using System;
using System.Configuration;
using M3C.Finance.BinanceSdk.Enumerations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace M3C.Finance.BinanceSdk.Tests
{
    [TestClass]
    public class PublicRequestTests
    {
        private BinanceClient _client;

        [TestInitializeAttribute]
        public void Setup()
        {
            _client = new BinanceClient(ConfigurationManager.AppSettings["BinanceApiKey"],
                ConfigurationManager.AppSettings["BinanceApiSecret"]);
        }

        [TestMethod]
        public void TestPing()
        {
            var response = _client.Ping();
            Assert.IsTrue(response);
        }

        [TestMethod]
        public void TestTime()
        {
            var response = _client.Time();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestDepthWithInvalidSymbol()
        {
            var response = _client.Depth("NEO");
        }

        [TestMethod]
        public void TestDepth()
        {
            var response = _client.Depth("NEOBTC");
            Assert.IsTrue(response.Asks.Count > 0);
            Assert.IsTrue(response.Bids.Count > 0);
        }

        [TestMethod]
        public void TestDepthWithLimit()
        {
            var response = _client.Depth("NEOBTC",10);
            Assert.IsTrue(response.Asks.Count <= 10);
            Assert.IsTrue(response.Bids.Count <= 10);
        }

        [TestMethod]
        public void TestAggTrades()
        {
            var response = _client.AggregateTrades("NEOBTC");
        }

        [TestMethod]
        public void TestKLines()
        {
            var response = _client.KLines("NEOBTC",KlineInterval.Month1);
        }

        [TestMethod]
        public void TestDailyTicker()
        {
            var response = _client.TickerDaily("NEOBTC");
        }

        [TestMethod]
        public void TestAllPricesTicker()
        {
            var response = _client.TickerAllPrices();
        }

        [TestMethod]
        public void TestAllBookTickers()
        {
            var response = _client.AllBookTickers();
        }

    }
}
