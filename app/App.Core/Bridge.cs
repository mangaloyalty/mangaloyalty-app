using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using App.Core.Shared;
using App.Core.Shared.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace App.Core
{
    public class Bridge
    {
        private readonly ICallback _callback;
        private readonly object _source;

        #region Abstracts

        private object ProcessRequest(RequestDataModel model)
        {
            var pieces = model.EventName.ToLowerInvariant().Split('.').ToList();
            var source = _source;

            while (pieces.Count > 1)
            {
                var p = source.GetType().GetRuntimeProperties().FirstOrDefault(x => x.Name.ToLower() == pieces[0]);
                if (p == null) break;
                source = p.GetValue(source);
                pieces.RemoveAt(0);
            }

            while (pieces.Count == 1)
            {
                var m = source.GetType().GetRuntimeMethods().FirstOrDefault(x => x.Name.ToLower() == pieces[0]);
                if (m == null) break;
                var p = m.GetParameters();
                return m.Invoke(source, p.Length != 0 ? new[] {model.Value.ToObject(p[0].ParameterType)} : null);
            }

            throw new Exception($"Unknown event name: {model.EventName}");
        }

        private async Task<JToken> ProcessResponseAsync(RequestDataModel model, object response)
        {
            if (!(response is Task task))
            {
                var result = JsonConvert.SerializeObject(response);
                return await _callback.EvaluateAsync($"{model.CallbackName}(true, {result})");
            }

            try
            {
                await task;
                if (!task.GetType().IsConstructedGenericType) return await _callback.EvaluateAsync($"{model.CallbackName}(true)");
                var result = JsonConvert.SerializeObject(task.GetType().GetRuntimeProperty("Result").GetValue(task));
                return await _callback.EvaluateAsync($"{model.CallbackName}(true, {result})");
            }
            catch (Exception ex)
            {
                var message = JsonConvert.SerializeObject(ex);
                return await _callback.EvaluateAsync($"{model.CallbackName}(false, {message})");
            }
        }

        #endregion

        #region Constructor

        public Bridge(ICallback callback, object source)
        {
            _callback = callback;
            _source = source;
        }

        #endregion

        #region Methods

        public async Task<JToken> DispatchAsync(string eventName, object value = null)
        {
            eventName = JsonConvert.SerializeObject(eventName);
            value = JsonConvert.SerializeObject(value);
            return await _callback.EvaluateAsync($"oni.dispatchAsync({eventName}, {value})");
        }

        public async Task<JToken> ProcessRequestAsync(RequestDataModel model)
        {
            try
            {
                return await ProcessResponseAsync(model, ProcessRequest(model));
            }
            catch (Exception ex)
            {
                var message = JsonConvert.SerializeObject(ex.Message);
                return await _callback.EvaluateAsync($"{model.CallbackName}(false, {message})");
            }
        }

        public async Task<JToken> ProcessRequestAsync(string json)
        {
            return await ProcessRequestAsync(JsonConvert.DeserializeObject<RequestDataModel>(json));
        }

        #endregion
    }
}