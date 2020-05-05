using Newtonsoft.Json;

namespace App.Core.Server.Models
{
    public class ResourceDataModel
    {
        [JsonProperty]
        public string AbsolutePath { get; set; }
    }
}