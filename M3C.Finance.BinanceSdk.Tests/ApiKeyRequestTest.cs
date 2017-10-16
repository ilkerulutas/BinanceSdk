using System;
using System.Configuration;
using System.Threading;
using M3C.Finance.BinanceSdk.Enumerations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace M3C.Finance.BinanceSdk.Tests
{
    [TestClass]
    public class ApiKeyRequestTests
    {
        private BinanceClient _client;

        [TestInitializeAttribute]
        public void Setup()
        {
            _client = new BinanceClient(ConfigurationManager.AppSettings["BinanceApiKey"],
                ConfigurationManager.AppSettings["BinanceApiSecret"]);
        }

        [TestMethod]
        public void TestStartUserDataStream()
        {
            var listenKey = _client.StartUserDataStreamSync();
            Assert.IsFalse(string.IsNullOrEmpty(listenKey));

            var listenKeyFromAsync = _client.StartUserDataStream();
            Assert.IsFalse(string.IsNullOrEmpty(listenKeyFromAsync.Result));
        }

        [TestMethod]
        public void TestKeepAliveUserDataStream()
        {
            var listenKey = _client.StartUserDataStream();
            Thread.Sleep(3000);
            var response = _client.KeepAliveUserDataStream(listenKey.Result);
            Assert.IsTrue(response.Result);
        }

        [TestMethod]
        public void TestCloseUserDataStream()
        {
            var listenKey = _client.StartUserDataStream();
            Thread.Sleep(3000);
            var response = _client.CloseUserDataStream(listenKey.Result);
            Assert.IsTrue(response.Result);
        }

    }
}
