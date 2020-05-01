using Newtonsoft.Json;

namespace App.Core.Models.Plugins
{
    public class ResourceDataModel
    {
        [JsonProperty]
        public string AbsolutePath { get; set; }
    }
}