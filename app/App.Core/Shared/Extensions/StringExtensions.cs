using Newtonsoft.Json.Linq;

namespace App.Core.Shared.Extensions
{
    public static class StringExtensions
    {
        public static JToken ParseJson(this string value)
        {
            return JToken.Parse(value);
        }

        public static T ParseJson<T>(this string value)
        {
            return value.ParseJson().ToObject<T>();
        }
    }
}