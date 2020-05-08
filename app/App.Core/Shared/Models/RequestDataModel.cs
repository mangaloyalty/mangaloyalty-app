using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace App.Core.Shared.Models
{
    public class RequestDataModel
    {
        [JsonProperty]
        public string Callback { get; set; }

        [JsonProperty]
        public string Key { get; set; }

        [JsonProperty]
        public JToken Value { get; set; }
    }
}