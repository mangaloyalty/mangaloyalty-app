using Newtonsoft.Json;

namespace App.Core.Server.Models
{
    public class BrowserEvaluateDataModel
    {
        [JsonProperty]
        public string Id { get; set; }

        [JsonProperty]
        public string Invoke { get; set; }
    }
}