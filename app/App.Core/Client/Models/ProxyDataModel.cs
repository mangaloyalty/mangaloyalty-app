using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace App.Core.Client.Models
{
    public class ProxyDataModel
    {
        [JsonProperty]
        public string EventSource { get; set; }

        [JsonProperty]
        public string FunctionName { get; set; }

        [JsonProperty]
        public JToken Parameters { get; set; }
    }
}