﻿using Newtonsoft.Json;

namespace App.Core.Server.Models
{
    public class ResourceWriteFileDataModel
    {
        [JsonProperty]
        public string AbsolutePath { get; set; }

        [JsonProperty]
        public string Buffer { get; set; }
    }
}