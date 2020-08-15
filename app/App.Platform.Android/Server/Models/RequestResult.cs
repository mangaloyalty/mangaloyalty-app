using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace App.Platform.Android.Server.Models
{
    public class RequestResult
    {
        [JsonProperty("content")]
        public JToken Content { get; set; }

        [JsonProperty("content64")]
        public string Content64 { get; set; }

        [JsonProperty("headers")]
        public Dictionary<string, string> Headers { get; set; }

        [JsonProperty("statusCode")]
        public int StatusCode { get; set; }
    }
}