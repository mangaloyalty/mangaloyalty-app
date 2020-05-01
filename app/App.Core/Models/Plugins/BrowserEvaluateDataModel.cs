using Newtonsoft.Json;

namespace App.Core.Models.Plugins
{
    public class BrowserEvaluateDataModel
    {
        [JsonProperty]
        public string Id { get; set; }

        [JsonProperty]
        public string Invoke { get; set; }
    }
}