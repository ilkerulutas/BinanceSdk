using System;
using System.Configuration;
using System.Linq;
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
            _client = new BinanceClient();
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
            Assert.IsTrue(response.ServerTime > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(BinanceRestApiException))]
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
            Assert.IsTrue(response.Any());
            Assert.IsTrue(response[0].AggregateTradeId > 0);
        }

        [TestMethod]
        public void TestKLines()
        {
            var response = _client.KLines("NEOBTC",KlineInterval.Month1).ToList();
            Assert.IsTrue(response.Any());
            Assert.IsTrue(response[0].OpenTime > 0);
            Assert.IsTrue(response[0].CloseTime > 0);
        }

        [TestMethod]
        public void TestDailyTicker()
        {
            var response = _client.TickerDaily("NEOBTC");
            Assert.IsTrue(response.AskPrice > 0);
            Assert.IsTrue(response.BidPrice > 0);
        }

        [TestMethod]
        public void TestAllPricesTicker()
        {
            var response = _client.TickerAllPrices();
            Assert.IsTrue(response.Any());
        }

        [TestMethod]
        public void TestAllBookTickers()
        {
            var response = _client.AllBookTickers();
            Assert.IsTrue(response.Any());
        }

    }
}
