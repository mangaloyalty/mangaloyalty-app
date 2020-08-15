using System.Collections.Specialized;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace App.Platform.Android.Server.Extensions
{
    public static class NameValueCollectionExtensions
    {
        public static JToken ToJsonDictionary(this NameValueCollection source, bool lowerCase = false)
        {
            var result = new JObject();
            foreach (var key in source.AllKeys.Where(x => x != null)) result.Add(lowerCase ? key.ToLowerInvariant() : key, source[key]);
            return result;
        }
    }
}