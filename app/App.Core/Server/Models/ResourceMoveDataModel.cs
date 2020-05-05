using Newtonsoft.Json;

namespace App.Core.Server.Models
{
    public class ResourceMoveDataModel
    {
        [JsonProperty]
        public string AbsoluteFromPath { get; set; }

        [JsonProperty]
        public string AbsoluteToPath { get; set; }
    }
}