using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using App.Core.Interfaces;
using App.Core.Models;
using App.Core.Plugins;
using Newtonsoft.Json;

namespace App.Core
{
    public sealed class Bridge
    {
        private readonly IClient _client;
        private readonly ICorePlugin _corePlugin;
        private readonly List<KeyValuePair<string, object>> _queue;

        #region Abstracts

        private void EmptyQueue()
        {
            while (_queue.Count != 0)
            {
                DispatchEvent(_queue[0].Key, _queue[0].Value);
                _queue.RemoveAt(0);
            }
        }

        private object ProcessRequest(RequestDataModel model)
        {
            var pieces = model.EventName.ToLowerInvariant().Split('.').ToList();
            var source = _corePlugin as object;

            while (pieces.Count > 1)
            {
                var pi = source.GetType().GetRuntimeProperties().FirstOrDefault(x => x.Name.ToLower() == pieces[0]);
                if (pi == null) break;
                source = pi.GetValue(source);
                pieces.RemoveAt(0);
            }

            while (pieces.Count == 1)
            {
                var mi = source.GetType().GetRuntimeMethods().FirstOrDefault(x => x.Name.ToLower() == pieces[0]);
                if (mi == null) break;
                var pi = mi.GetParameters();
                return mi.Invoke(source, pi.Length != 0 ? new[] {model.Value.ToObject(pi[0].ParameterType)} : null);
            }

            throw new Exception($"Unknown event name: {model.EventName}");
        }

        private void ProcessResponse(RequestDataModel model, object response)
        {
            if (!(response is Task task))
            {
                _client.Submit(model.CallbackName, new SubmitDataModel(true, response));
                return;
            }

            task.ContinueWith(x =>
            {
                if (x.IsFaulted)
                {
                    _client.Submit(model.CallbackName, new SubmitDataModel(false, string.Join(
                        Environment.NewLine,
                        x.Exception.InnerExceptions.Select(y => y.Message))));
                }
                else if (x.GetType().IsConstructedGenericType)
                {
                    _client.Submit(model.CallbackName, new SubmitDataModel(true, x.GetType()
                        .GetRuntimeProperty(nameof(Task<object>.Result))
                        .GetValue(x)));
                }
                else
                {
                    _client.Submit(model.CallbackName, new SubmitDataModel(true));
                }
            });
        }

        #endregion

        #region Constructor

        public Bridge(IClient client, ICorePlugin corePlugin)
        {
            _client = client;
            _corePlugin = corePlugin;
            _queue = new List<KeyValuePair<string, object>>();
        }

        #endregion

        #region Methods

        public void DispatchEvent(string eventName, object value = null)
        {
            if (IsActive)
            {
                _client.Submit("oni.dispatchEvent", new SubmitDataModel(eventName, value));
            }
            else
            {
                _queue.Add(new KeyValuePair<string, object>(eventName, value));
            }
        }

        public void ProcessRequest(string json)
        {
            var model = JsonConvert.DeserializeObject<RequestDataModel>(json);
            var response = null as object;

            try
            {
                response = ProcessRequest(model);
            }
            catch (Exception ex)
            {
                _client.Submit(model.CallbackName, new SubmitDataModel(false, ex.Message));
            }
            finally
            {
                ProcessResponse(model, response);
            }
        }

        public void UpdateState(bool isActive)
        {
            if (isActive)
            {
                IsActive = true;
                EmptyQueue();
                DispatchEvent("focus");
            }
            else
            {
                DispatchEvent("blur");
                IsActive = false;
            }
        }

        #endregion

        #region Properties

        public bool IsActive { get; private set; }

        #endregion
    }
}