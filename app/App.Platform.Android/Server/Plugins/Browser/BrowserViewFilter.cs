using System.Text.RegularExpressions;
using App.Platform.Android.Server.Plugins.Browser.Enumerators;

namespace App.Platform.Android.Server.Plugins.Browser
{
    public static class BrowserViewFilter
    {
        public static FilterState GetState(string host)
        {
            // Initialize basic filters.
            if (Regex.IsMatch(host, @"^cloudflare\.com$")) return FilterState.Allow;
            if (Regex.IsMatch(host, @"\.cloudflare\.com$")) return FilterState.Allow;
            
            // Initialize batoto filters.
            if (Regex.IsMatch(host, @"^bato\.to$")) return FilterState.Allow;
            if (Regex.IsMatch(host, @"^static\.bato\.to$")) return FilterState.Allow;
            if (Regex.IsMatch(host, @"\.bato\.to$")) return FilterState.Cache;

            // Initialize fanfox filters.
            if (Regex.IsMatch(host, @"^fanfox\.net$")) return FilterState.Allow;
            if (Regex.IsMatch(host, @"^mangafox\.me$")) return FilterState.Allow;
            if (Regex.IsMatch(host, @"^static\.fanfox\.net$")) return FilterState.Allow;
            if (Regex.IsMatch(host, @"^static\.mangafox\.me")) return FilterState.Allow;
            if (Regex.IsMatch(host, @"\.fanfox\.net$")) return FilterState.Cache;
            if (Regex.IsMatch(host, @"\.mangafox\.me$")) return FilterState.Cache;
            return FilterState.Block;
        }
    }
}