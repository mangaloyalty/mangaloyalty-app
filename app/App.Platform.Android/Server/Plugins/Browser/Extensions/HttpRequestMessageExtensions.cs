using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Android.Webkit;

namespace App.Platform.Android.Server.Plugins.Browser.Extensions
{
    public static class HttpRequestMessageExtensions
    {
        public static void CopyCookies(this HttpRequestMessage message, CookieManager cookieManager)
        {
            var cookie = cookieManager.GetCookie(message.RequestUri.ToString());
            if (cookie != null) message.Headers.Add(nameof(cookie), cookie);
        }

        public static void CopyHeaders(this HttpRequestMessage message, IDictionary<string, string> headers, params string[] skipHeaders)
        {
            foreach (var (key, value) in headers)
            {
                if (skipHeaders.Any(x => string.Equals(key, x, StringComparison.InvariantCultureIgnoreCase))) continue;
                message.Headers.Add(key, value);
            }
        }
    }
}