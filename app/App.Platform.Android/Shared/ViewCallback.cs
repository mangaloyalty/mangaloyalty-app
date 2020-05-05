using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Android.Webkit;
using App.Core.Shared;
using App.Platform.Android.Utilities;
using Java.Interop;
using Newtonsoft.Json.Linq;

namespace App.Platform.Android.Shared
{
    public class ViewCallback : Java.Lang.Object, ICallback
    {
        private readonly Controller _controller;
        private readonly ConcurrentDictionary<string, TaskCompletionSource<JToken>> _receivers;
        private readonly string _token;
        private readonly WebView _view;
        private int _previousId;

        #region Abstracts

        private void Initialize()
        {
            _view.AddJavascriptInterface(this, _token);
        }

        #endregion

        #region Constructor

        public ViewCallback(Controller controller, WebView view)
        {
            _controller = controller;
            _receivers = new ConcurrentDictionary<string, TaskCompletionSource<JToken>>();
            _token = char.ConvertFromUtf32(new Random().Next(97, 122)) + new Random().Next(100, 999999);
            _view = view;
            Initialize();
        }

        #endregion

        #region Methods

        [Export("resolve")]
        [JavascriptInterface]
        public void ReceiveResolve(string id, string json)
        {
            if (!_receivers.TryGetValue(id, out var receiver)) return;
            receiver.TrySetResult(json != null ? JToken.Parse(json) : null);
        }

        [Export("reject")]
        [JavascriptInterface]
        public void ReceiveReject(string id)
        {
            if (!_receivers.TryGetValue(id, out var receiver)) return;
            receiver.TrySetCanceled();
        }

        #endregion

        #region Implementation of IClient

        public async Task<JToken> EvaluateAsync(string script)
        {
            // Initialize the identifier.
            var id = Interlocked.Increment(ref _previousId).ToString();
            var tcs = new TimeoutTaskCompletionSource<JToken>(30);
            if (!_receivers.TryAdd(id, tcs)) return null;

            // Initialize the script.
            var runnable = Script.Replace("$id", id).Replace("$script", script).Replace("$token", _token);
            await _controller.RunAsync(() => _view.EvaluateJavascript(runnable, null));
            return await tcs.Task;
        }

        #endregion

        #region Script

        private const string Script = @"Promise.resolve($script).then((value) => {
            $token.resolve($id, JSON.stringify(value));
        }, () => {
            $token.reject($id);
        });";

        #endregion
    }
}