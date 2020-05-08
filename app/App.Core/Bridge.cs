using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using App.Core.Shared.Extensions;
using App.Core.Shared.Interfaces;
using App.Core.Shared.Models;
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
            var pieces = model.Key.ToLowerInvariant().Split('.').ToList();
            var source = _source;

            while (pieces.Count > 1)
            {
                var pi = source.GetType().GetRuntimeProperties().FirstOrDefault(x => x.Name.ToLowerInvariant() == pieces[0]);
                if (pi == null) break;
                source = pi.GetValue(source);
                pieces.RemoveAt(0);
            }

            while (pieces.Count == 1)
            {
                var mi = source.GetType().GetRuntimeMethods().FirstOrDefault(x => x.Name.ToLowerInvariant() == pieces[0]);
                if (mi == null) break;
                var pi = mi.GetParameters();
                return mi.Invoke(source, pi.Length != 0 ? new[] {model.Value.ToObject(pi[0].ParameterType)} : null);
            }

            throw new Exception(model.Key);
        }

        private async Task<JToken> ProcessResponseAsync(RequestDataModel model, object response)
        {
            try
            {
                if (!(response is Task task)) return await _callback.EvaluateAsync($"{model.Callback}(true, {response.ToJson()})");
                await task;
                return await _callback.EvaluateAsync($"{model.Callback}(true, {task.ToJson()})");
            }
            catch (Exception ex)
            {
                return await _callback.EvaluateAsync($"{model.Callback}(false, {ex.ToJson()})");
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

        public async Task<JToken> EventAsync(string eventName, object value = null)
        {
            return await _callback.EvaluateAsync($"oni.dispatchAsync({eventName.ToJson()}, {value.ToJson()})");
        }

        public async Task<JToken> RequestAsync(RequestDataModel model)
        {
            try
            {
                return await ProcessResponseAsync(model, ProcessRequest(model));
            }
            catch (Exception ex)
            {
                return await _callback.EvaluateAsync($"{model.Callback}(false, {ex.ToJson()})");
            }
        }

        public async Task<JToken> RequestAsync(string json)
        {
            return await RequestAsync(json.ParseJson<RequestDataModel>());
        }

        #endregion
    }
}