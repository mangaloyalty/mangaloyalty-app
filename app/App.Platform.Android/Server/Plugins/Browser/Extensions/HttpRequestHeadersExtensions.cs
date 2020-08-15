using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace App.Platform.Android.Server.Plugins.Browser.Extensions
{
    public static class HttpResponseHeadersExtensions
    {
        public static IDictionary<string, string> ToDictionary(this HttpResponseHeaders headers, params string[] skipHeaders)
        {
            return headers
                .Where(x => !skipHeaders.Any(y => string.Equals(y, x.Key, StringComparison.InvariantCultureIgnoreCase)))
                .ToDictionary(x => x.Key, x => string.Join(",", x.Value));
        }
    }
}