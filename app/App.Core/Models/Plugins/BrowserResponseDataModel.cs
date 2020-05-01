using Newtonsoft.Json;

namespace App.Core.Models.Plugins
{
    public class BrowserResponseDataModel
    {
        [JsonProperty]
        public string Id { get; set; }

        [JsonProperty]
        public string Url { get; set; }
    }
}