using Newtonsoft.Json;

namespace App.Core.Models.Plugins
{
    public class TraceDataModel
    {
        [JsonProperty]
        public string Message { get; set; }
    }
}