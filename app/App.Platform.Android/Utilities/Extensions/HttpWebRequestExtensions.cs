using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using Android.Webkit;

namespace App.Platform.Android.Utilities.Extensions
{
    public static class HttpWebRequestExtensions
    {
        public static void CopyCookies(this HttpWebRequest request, CookieManager cookieManager)
        {
            var cookie = cookieManager.GetCookie(request.RequestUri.ToString());
            if (cookie != null) request.Headers[nameof(cookie)] = cookie;
        }

        public static void CopyHeaders(this HttpWebRequest request, IDictionary<string, string> headers)
        {
            foreach(var (key, value) in headers)
            {
                switch (key.ToLowerInvariant())
                {
                    case "accept":
                        request.Accept = value;
                        break;
                    case "connection":
                        request.Connection = value;
                        break;
                    case "content-length":
                        if (!long.TryParse(value, out var contentLength)) return;
                        request.ContentLength = contentLength;
                        break;
                    case "content-type":
                        request.ContentType = value;
                        break;
                    case "expect":
                        request.Expect = value;
                        break;
                    case "date":
                        if (!DateTime.TryParse(value, CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.None, out var date)) continue;
                        request.Date = date;
                        break;
                    case "host":
                        request.Host = value;
                        break;
                    case "if-modified-since":
                        if (!DateTime.TryParse(value, CultureInfo.InvariantCulture.DateTimeFormat, DateTimeStyles.None, out var ifModifiedSince)) continue;
                        request.IfModifiedSince = ifModifiedSince;
                        break;
                    case "range":
                        break;
                    case "referer":
                        request.Referer = value;
                        break;
                    case "transfer-encoding":
                        request.SendChunked = true;
                        request.TransferEncoding = value;
                        break;
                    case "user-agent":
                        request.UserAgent = value;
                        break;
                    default:
                        request.Headers[key] = value;
                        break;
                }
            }
        }
    }
}