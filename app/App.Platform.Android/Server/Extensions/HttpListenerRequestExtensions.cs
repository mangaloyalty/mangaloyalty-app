using System.Net;
using Newtonsoft.Json.Linq;

namespace App.Platform.Android.Server.Extensions
{
    public static class HttpListenerRequestExtensions
    {
        public static JToken ToModel(this HttpListenerRequest request)
        {
            return new JObject(
                new JProperty("method", request.HttpMethod),
                new JProperty("path", request.Url.LocalPath),
                new JProperty("context", new JObject(
                    new JProperty("header", request.Headers.ToJsonDictionary(true)),
                    new JProperty("query", request.QueryString.ToJsonDictionary()))));
        }
    }
}