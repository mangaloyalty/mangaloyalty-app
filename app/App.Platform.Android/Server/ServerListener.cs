using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using App.Platform.Android.Server.Extensions;
using App.Platform.Android.Server.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace App.Platform.Android.Server
{
    public class ServerListener : IDisposable
    {
        private readonly ServerCore _core;
        private readonly HttpListener _listener;

        #region Abstracts

        private void Initialize()
        {
            _listener.Start();
            Task.Factory.StartNew(ListenAsync, TaskCreationOptions.LongRunning);
        }

        private async Task ListenAsync()
        {
            // TODO: Dispose?
            while (_listener.IsListening)
            {
                var context = await _listener.GetContextAsync();
                _ = Task.Run(() => ProcessAsync(context));
            }
        }

        private async Task ProcessAsync(HttpListenerContext context)
        {
            try
            {
                var ev = new JObject(
                    new JProperty("method", context.Request.HttpMethod),
                    new JProperty("path", context.Request.Url.LocalPath),
                    new JProperty("context", new JObject(
                        new JProperty("header", context.Request.Headers.ToJsonDictionary(true)),
                        new JProperty("query", context.Request.QueryString.ToJsonDictionary())
                    ))
                );

                var result = (await _core.RequestAsync(ev)).ToObject<RequestResult>();
                context.Response.StatusCode = result.StatusCode != 0 ? result.StatusCode : 200;
                
                if (result.Headers != null)
                {
                    foreach (var (key, value) in result.Headers)
                    {
                        context.Response.AddHeader(key, value);
                    }
                }

                if (!string.IsNullOrEmpty(result.Content64))
                {
                    await using var os = context.Response.OutputStream;
                    await os.WriteAsync(Convert.FromBase64String(result.Content64));
                    await os.FlushAsync();
                }
                else if (result.Content != null)
                {
                    context.Response.AddHeader("Content-Type", "application/json");
                    await using var os = context.Response.OutputStream;
                    await os.WriteAsync(Encoding.UTF8.GetBytes(result.Content.ToString(Formatting.Indented)));
                    await os.FlushAsync();
                }

                context.Response.Close();
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                await using var os = context.Response.OutputStream;
                await os.WriteAsync(Encoding.UTF8.GetBytes(ex.Message));
                await os.FlushAsync();
            }
        }

        #endregion

        #region Constructor

        public ServerListener(ServerCore core)
        {
            _core = core;
            _listener = new HttpListener();
            _listener.Prefixes.Add("http://+:7783/"); // TODO: Localhost-only!
            Initialize();
        }

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            _listener.Stop();
        }

        #endregion
    }
}