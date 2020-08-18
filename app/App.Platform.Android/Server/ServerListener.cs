using System;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using App.Platform.Android.Server.Extensions;
using App.Platform.Android.Server.Models;
using Newtonsoft.Json;

namespace App.Platform.Android.Server
{
    public class ServerListener : IDisposable
    {
        private readonly ServerCore _core;
        private readonly HttpListener _listener;

        #region Abstracts

        private void Initialize()
        {
            _listener.Prefixes.Add("http://+:7783/");
            _listener.Start();
            Task.Factory.StartNew(ListenAsync, TaskCreationOptions.LongRunning);
        }

        private async Task ListenAsync()
        {
            while (_listener.IsListening)
            {
                try
                {
                    var context = await _listener.GetContextAsync();
                    _ = Task.Run(() => ProcessAsync(context));
                }
                catch (ObjectDisposedException)
                {
                    break;
                }
            }
        }
        
        private async Task ProcessAsync(HttpListenerContext context)
        {
            if (!context.Request.IsLocal && !Debugger.IsAttached)
            {
                context.Response.StatusCode = 403;
                context.Response.AddHeader("Access-Control-Allow-Origin", "*");
                context.Response.Close();
                return;
            }

            try
            {
                var model = context.Request.ToModel();
                var response = await _core.RequestAsync(model);
                var result = response.ToObject<RequestResult>();
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
                context.Response.AddHeader("Access-Control-Allow-Origin", "*");
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