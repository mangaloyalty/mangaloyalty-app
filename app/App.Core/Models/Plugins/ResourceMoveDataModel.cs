using Newtonsoft.Json;

namespace App.Core.Models.Plugins
{
    public class ResourceMoveDataModel
    {
        [JsonProperty]
        public string AbsoluteFromPath { get; set; }

        [JsonProperty]
        public string AbsoluteToPath { get; set; }
    }
}