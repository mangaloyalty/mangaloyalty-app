using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace App.Core.Server.Models
{
    public class ResourceWriteJsonDataModel
    {
        [JsonProperty]
        public string AbsolutePath { get; set; }

        [JsonProperty]
        public JToken Value { get; set; }
    }
}