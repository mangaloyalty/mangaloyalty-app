using Newtonsoft.Json;

namespace App.Core.Models.Plugins
{
    public class BrowserNavigateDataModel
    {
        [JsonProperty]
        public string Id { get; set; }

        [JsonProperty]
        public string Url { get; set; }
    }
}