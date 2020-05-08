using Newtonsoft.Json;

namespace App.Core.Shared.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object value)
        {
            return JsonConvert.SerializeObject(value);
        }
    }
}