using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace App.Core.Client.Models
{
    public class ProxyDataModel
    {
        [JsonProperty]
        public string Key { get; set; }
        
        [JsonProperty]
        public JToken Value { get; set; }
    }
}