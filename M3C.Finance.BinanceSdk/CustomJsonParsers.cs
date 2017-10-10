using System.Collections.Generic;
using M3C.Finance.BinanceSdk.ResponseObjects;
using Newtonsoft.Json.Linq;

namespace M3C.Finance.BinanceSdk
{
    public static class CustomJsonParsers
    {
        public static DepthResponse DepthResponseParser(string input)
        {
            var obj = JObject.Parse(input);
            var bidItems = new List<OrderRecord>();
            foreach (var bidItem in (JArray)obj["bids"])
            {
                bidItems.Add(new OrderRecord
                {
                    Price = (decimal)bidItem[0],
                    Quantity = (decimal)bidItem[1]
                });
            }
            var askItems = new List<OrderRecord>();
            foreach (var askItem in (JArray)obj["asks"])
            {
                askItems.Add(new OrderRecord
                {
                    Price = (decimal)askItem[0],
                    Quantity = (decimal)askItem[1]
                });
            }

            return new DepthResponse
            {
                LastUpdateId = (long)obj["lastUpdateId"],
                Asks = askItems,
                Bids = bidItems
            };
        }

        public static List<KLinesResponseItem> KLineResponseParser(string input)
        {
            var jArray = JArray.Parse(input);
            var resultList = new List<KLinesResponseItem>();
            foreach (var item in jArray)
            {
                resultList.Add(new KLinesResponseItem
                {
                    OpenTime = (long)item[0],
                    Open = (decimal)item[1],
                    High = (decimal)item[2],
                    Low = (decimal)item[3],
                    Close = (decimal)item[4],
                    Volume = (decimal)item[5],
                    CloseTime = (long)item[6],
                    QuoteAssetVolume = (decimal)item[7],
                    NumberOfTrades = (int)item[8],
                    TakerBuyBaseAssetVolume = (decimal)item[9],
                    TakerBuyQuoteAssetVolume = (decimal)item[10]
                });
            }
            return resultList;
        }

        public static WebSocketDepthMessage DepthMessageParser(string input)
        {
            var response = JObject.Parse(input);
            return new WebSocketDepthMessage
            {
                EventType = (string)response["e"],
                EventTime = (long)response["E"],
                Symbol = (string)response["s"],
                UpdateId = (long)response["u"],
                AskDepthDelta = AskBidParser((JArray)response["a"]),
                BidDepthDelta = AskBidParser((JArray)response["b"])
            };
        }

        private static List<OrderRecord> AskBidParser(JArray array)
        {
            var resultList = new List<OrderRecord>();
            foreach (var item in array)
            {
                resultList.Add(new OrderRecord
                {
                    Price = (decimal)item[0],
                    Quantity = (decimal)item[1]
                });
            }
            return resultList;
        }
    }
}
