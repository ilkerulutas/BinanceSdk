using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace M3C.Finance.BinanceSdk
{
    public partial class BinanceClient
    {
        private readonly string _apiKey;
        private readonly string _apiSecret;

        private const string BaseUrl = "https://www.binance.com/api";

        /// <summary>
        /// Binance Rest Api Client
        /// </summary>
        /// <param name="apiKey">Binance Api Key</param>
        /// <param name="apiSecret">Binance Api Secret</param>
        public BinanceClient(string apiKey, string apiSecret)
        {
            _apiKey = apiKey;
            _apiSecret = apiSecret;
        }

        private delegate T ResponseParseHandler<T>(string input);

        private static long CurrentMilliseconds
        {
            get
            {
                var epochTicks = new DateTime(1970, 1, 1).Ticks;
                var unixTime = (DateTime.UtcNow.Ticks - epochTicks) / TimeSpan.TicksPerMillisecond;
                return unixTime;
            }
        }
        
        private T SendRequest<T>(string methodName, string version, ApiMethodType apiMethod, HttpMethod httpMethod,
            Dictionary<string, string> parameters = null, ResponseParseHandler<T> customHandler = null)
        {
            
            if (parameters == null)
            {
                parameters = new Dictionary<string, string>();
            }

            if (apiMethod == ApiMethodType.Signed)
            {
                var timestamp = CurrentMilliseconds;
                parameters.Add("timestamp", timestamp.ToString(CultureInfo.InvariantCulture));
                var parameterTextForSignature = GetParameterText(parameters);

                var signature = new HMACSHA256(Encoding.UTF8.GetBytes(_apiSecret))
                    .ComputeHash(Encoding.UTF8.GetBytes(parameterTextForSignature));
                parameters.Add("signature", GetHexString(signature));
            }
            var parameterTextPrefix = parameters.Count > 0 ? "?" : string.Empty;
            var parameterText = GetParameterText(parameters);

            var getRequestUrl = $"{BaseUrl}/{version}/{methodName}{parameterTextPrefix}{parameterText}";
            var postRequestUrl = $"{BaseUrl}/{version}/{methodName}";

            string response;

            using (var client = new WebClient())
            {
                if (apiMethod == ApiMethodType.Signed || apiMethod == ApiMethodType.ApiKey)
                {
                    client.Headers.Add("X-MBX-APIKEY", _apiKey);
                }

                try
                {
                    response = httpMethod == HttpMethod.Get ?
                        client.DownloadString(getRequestUrl) :
                        client.UploadString(postRequestUrl, httpMethod.Method, parameterText);
                }
                catch (WebException webException)
                {
                    using (var reader = new StreamReader(webException.Response.GetResponseStream(), Encoding.UTF8))
                    {
                        var errorContent = reader.ReadToEnd();
                        var errorObject = JObject.Parse(errorContent);
                        var errorCode = (int)errorObject["code"];
                        var errorMessage = (string)errorObject["msg"];
                        throw new BinanceRestApiException(errorCode,errorMessage);
                    }
                }
            }
            return customHandler != null ? customHandler(response) : JsonConvert.DeserializeObject<T>(response);
        }

        
        private static string GetHexString(byte[] bytes)
        {
            var builder = new StringBuilder(bytes.Length * 2);
            foreach (var b in bytes)
            {
                builder.Append($"{b:x2}");
            }
            return builder.ToString();
        }

        private static string GetParameterText(Dictionary<string,string> parameters)
        {
            if (parameters.Count == 0)
            {
                return string.Empty;
            }

            var builder = new StringBuilder();
            foreach (var item in parameters)
            {
                builder.Append($"&{item.Key}={item.Value}");
            }
            return builder.Remove(0,1).ToString();
        }
        
        private enum ApiMethodType
        {
            None = 0,
            ApiKey = 1,
            Signed = 2
        }

    }
}
