using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using M3C.Finance.BinanceSdk.Enumerations;
using M3C.Finance.BinanceSdk.ResponseObjects;

namespace M3C.Finance.BinanceSdk
{
    public partial class BinanceClient
    {
        public async Task<AccountResponse> GetAccountInfo(bool filterZeroBalaces = false)
        {
            var response = await SendRequest<AccountResponse>("account", ApiVersion.Version3, ApiMethodType.Signed, HttpMethod.Get);
            if (filterZeroBalaces)
            {
                response.Balances = response.Balances.Where(a => a.Free + a.Locked != 0).ToList();
            }
            return response;
        }

        public AccountResponse GetAccountInfoSync() => GetAccountInfo().Result;

        public async Task<NewOrderResponse> NewOrder(string symbol, OrderSide side, OrderType orderType, TimeInForce timeInForce, decimal quantity, decimal price, bool isTestOrder = false,
            string newClientOrderId = null, decimal? stopPrice = null, decimal? icebergQuantity = null, long? recvWindow = null)
        {
            var parameters = new Dictionary<string, string>
            {
                {"symbol", symbol},
                {"side", side},
                {"type", orderType},
                {"timeInForce", timeInForce},
                {"quantity", quantity.ToString(CultureInfo.InvariantCulture)},
                {"price", price.ToString(CultureInfo.InvariantCulture)}
            };

            CheckAndAddReceiveWindow(recvWindow, parameters);

            if (!string.IsNullOrEmpty(newClientOrderId))
            {
                parameters.Add("newClientOrderId", newClientOrderId);
            }
            if (stopPrice.HasValue)
            {
                parameters.Add("stopPrice", stopPrice.Value.ToString(CultureInfo.InvariantCulture));
            }
            if (icebergQuantity.HasValue)
            {
                parameters.Add("icebergQty", icebergQuantity.Value.ToString(CultureInfo.InvariantCulture));
            }
            return await SendRequest<NewOrderResponse>(isTestOrder ? "order/test" : "order", ApiVersion.Version3, ApiMethodType.Signed, HttpMethod.Post, parameters);
        }

        public NewOrderResponse NewOrderSync(string symbol, OrderSide side, OrderType orderType,
            TimeInForce timeInForce, decimal quantity, decimal price, bool isTestOrder = false,
            string newClientOrderId = null, decimal? stopPrice = null, decimal? icebergQuantity = null,
            long? recvWindow = null) =>
            NewOrder(symbol, side, orderType, timeInForce, quantity, price, isTestOrder, newClientOrderId, stopPrice,
                icebergQuantity, recvWindow).Result;

        public async Task<QueryOrderResponse> QueryOrder(string symbol, long? orderId, string clientOrderId = null, long? recvWindow = null)
        {
            var parameters = new Dictionary<string, string>
            {
                {"symbol", symbol},
            };

            CheckAndAddReceiveWindow(recvWindow, parameters);

            if (!orderId.HasValue && string.IsNullOrEmpty(clientOrderId))
            {
                throw new ArgumentException("Either orderId or clientOrderId should be set!");
            }

            if (orderId.HasValue)
            {
                parameters.Add("orderId", orderId.Value.ToString(CultureInfo.InvariantCulture));

            }
            if (!string.IsNullOrEmpty(clientOrderId))
            {
                parameters.Add("origClientOrderId", clientOrderId);
            }
            return await SendRequest<QueryOrderResponse>("order", ApiVersion.Version3, ApiMethodType.Signed, HttpMethod.Get, parameters);
        }

        public QueryOrderResponse QueryOrderSync(string symbol, long? orderId, string clientOrderId = null,
            long? recvWindow = null) =>
            QueryOrder(symbol, orderId, clientOrderId, recvWindow).Result;

        public async Task<IEnumerable<OrderInfo>> CurrentOpenOrders(string symbol, long? recvWindow = null)
        {
            var parameters = new Dictionary<string, string>
            {
                {"symbol", symbol},
            };

            CheckAndAddReceiveWindow(recvWindow, parameters);

            return await SendRequest<List<OrderInfo>>("openOrders", ApiVersion.Version3, ApiMethodType.Signed, HttpMethod.Get, parameters);
        }

        public IEnumerable<OrderInfo> CurrentOpenOrdersSync(string symbol, long? recvWindow = null) =>
            CurrentOpenOrders(symbol, recvWindow).Result;

        public async Task<IEnumerable<OrderInfo>> ListAllOrders(string symbol, long? orderId = null, int? limit = null, long? recvWindow = null)
        {
            var parameters = new Dictionary<string, string>
            {
                {"symbol", symbol},
            };

            CheckAndAddReceiveWindow(recvWindow, parameters);

            if (orderId.HasValue)
            {
                parameters.Add("orderId", orderId.Value.ToString(CultureInfo.InvariantCulture));
            }
            if (limit.HasValue)
            {
                parameters.Add("limit", limit.Value.ToString(CultureInfo.InvariantCulture));
            }
            return await SendRequest<List<OrderInfo>>("allOrders", ApiVersion.Version3, ApiMethodType.Signed, HttpMethod.Get, parameters);
        }

        public IEnumerable<OrderInfo> ListAllOrdersSync(string symbol, long? orderId = null, int? limit = null,
            long? recvWindow = null) =>
            ListAllOrders(symbol, orderId, limit, recvWindow).Result;

        public async Task<CancelOrderResponse> CancelOrder(string symbol, long? orderId = null, string originalClientOrderId = null, string newClientOrderId = null, long? recvWindow = null)
        {
            var parameters = new Dictionary<string, string>
            {
                {"symbol", symbol},
            };

            CheckAndAddReceiveWindow(recvWindow, parameters);

            if (orderId.HasValue)
            {
                parameters.Add("orderId", orderId.Value.ToString(CultureInfo.InvariantCulture));
            }
            if (!string.IsNullOrEmpty(originalClientOrderId))
            {
                parameters.Add("origClientOrderId", originalClientOrderId);
            }
            if (!string.IsNullOrEmpty(newClientOrderId))
            {
                parameters.Add("newClientOrderId", newClientOrderId);
            }
            return await SendRequest<CancelOrderResponse>("order", ApiVersion.Version3, ApiMethodType.Signed,
                HttpMethod.Delete, parameters);
        }

        public CancelOrderResponse CancelOrderSync(string symbol, long? orderId = null,
            string originalClientOrderId = null, string newClientOrderId = null, long? recvWindow = null) =>
            CancelOrder(symbol, orderId, originalClientOrderId, newClientOrderId, recvWindow).Result;

        public async Task<List<TradeInfo>> ListMyTrades(string symbol, int? limit = null, long? fromId = null, long? recvWindow = null)
        {
            var parameters = new Dictionary<string, string>
            {
                {"symbol", symbol},
            };

            CheckAndAddReceiveWindow(recvWindow, parameters);

            if (limit.HasValue)
            {
                parameters.Add("limit", limit.Value.ToString(CultureInfo.InvariantCulture));
            }
            if (fromId.HasValue)
            {
                parameters.Add("fromId", fromId.Value.ToString(CultureInfo.InvariantCulture));
            }
            return await SendRequest<List<TradeInfo>>("myTrades", ApiVersion.Version3, ApiMethodType.Signed, HttpMethod.Get, parameters);
        }

        public List<TradeInfo> ListMyTradesSync(string symbol, int? limit = null, long? fromId = null,
            long? recvWindow = null) =>
            ListMyTrades(symbol, limit, fromId, recvWindow).Result;

        private void CheckAndAddReceiveWindow(long? recvWindow, IDictionary<string, string> parameters)
        {
            if (recvWindow.HasValue)
            {
                parameters.Add("recvWindow", recvWindow.Value.ToString(CultureInfo.InvariantCulture));
            }
        }
    }
}
