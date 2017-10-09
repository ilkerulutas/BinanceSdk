using System.Collections.Generic;
using System.Net.Http;
using M3C.Finance.BinanceSdk.Enumerations;
using Newtonsoft.Json.Linq;

namespace M3C.Finance.BinanceSdk
{
    public partial class BinanceClient
    {
        public string StartUserDataStream()
        {
            var response = SendRequest<JObject>("userDataStream", ApiVersion.Version1, ApiMethodType.ApiKey, HttpMethod.Post);
            return (string)response["listenKey"];
        }

        public bool KeepAliveUserDataStream(string listenKey)
        {
            var response = SendRequest<JObject>("userDataStream", ApiVersion.Version1, ApiMethodType.ApiKey, HttpMethod.Put, new Dictionary<string, string>{ {"listenKey",listenKey} });
            return response.Type == JTokenType.Object && response.HasValues == false;
        }

        public bool CloseUserDataStream(string listenKey)
        {
            var response = SendRequest<JObject>("userDataStream", ApiVersion.Version1, ApiMethodType.ApiKey, HttpMethod.Delete, new Dictionary<string, string> { { "listenKey", listenKey } });
            return response.Type == JTokenType.Object && response.HasValues == false;
        }
    }
}
