using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace App.Core.Models.Plugins
{
    public class ResourceWriteJsonDataModel
    {
        [JsonProperty]
        public string AbsolutePath { get; set; }

        [JsonProperty]
        public JToken Value { get; set; }
    }
}