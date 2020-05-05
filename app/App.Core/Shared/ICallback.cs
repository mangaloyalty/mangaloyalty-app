﻿using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace App.Core.Shared
{
    public interface ICallback
    {
        Task<JToken> EvaluateAsync(string script);
    }
}