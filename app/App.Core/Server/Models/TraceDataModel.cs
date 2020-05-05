using Newtonsoft.Json;

namespace App.Core.Server.Models
{
    public class TraceDataModel
    {
        [JsonProperty]
        public string Message { get; set; }
    }
}