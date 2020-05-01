using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace App.Core.Models
{
    public sealed class SubmitDataModel
    {
        private readonly object[] _values;

        #region Abstract

        private static string Serialize<TItem>(TItem value)
        {
            var resolver = new CamelCasePropertyNamesContractResolver();
            var settings = new JsonSerializerSettings {ContractResolver = resolver};
            return JsonConvert.SerializeObject(value, settings);
        }

        #endregion

        #region Constructor

        public SubmitDataModel(params object[] values)
        {
            _values = values;
        }

        #endregion

        #region Properties

        public string InvokeData
        {
            get { return string.Join(", ", _values.Select(Serialize)); }
        }

        #endregion
    }
}