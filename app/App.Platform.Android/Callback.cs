using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Android.Content;
using Android.Webkit;
using App.Core.Shared;
using App.Core.Shared.Extensions;
using App.Core.Shared.Interfaces;
using Java.Interop;
using Newtonsoft.Json.Linq;

namespace App.Platform.Android
{
    public class Callback : Java.Lang.Object, ICallback
    {
        private readonly Context _context;
        private readonly ConcurrentDictionary<string, TaskCompletionSource<JToken>> _receiverTcs;
        private readonly string _token;
        private readonly WebView _view;
        private int _previousId;

        #region Abstracts

        private void Initialize()
        {
            _view.AddJavascriptInterface(this, _token);
        }

        private void EvaluateJavascript(string run, TaskCompletionSource<JToken> tcs)
        {
            try
            {
                _view.EvaluateJavascript(run, null);
            }
            catch (Exception ex)
            {
                tcs.TrySetException(ex);
            }
        }

        #endregion

        #region Constructor

        public Callback(Context context, WebView view)
        {
            _context = context;
            _receiverTcs = new ConcurrentDictionary<string, TaskCompletionSource<JToken>>();
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
            if (!_receiverTcs.TryRemove(id, out var receiverTcs)) return;
            receiverTcs.TrySetResult(json.ParseJson());
        }

        [Export("reject")]
        [JavascriptInterface]
        public void ReceiveReject(string id)
        {
            if (!_receiverTcs.TryRemove(id, out var receiverTcs)) return;
            receiverTcs.TrySetCanceled();
        }

        #endregion

        #region Implementation of ICallback

        public async Task<JToken> EvaluateAsync(string script)
        {
            // Initialize the identifier.
            var id = Interlocked.Increment(ref _previousId).ToString();
            var tcs = new TimeoutTaskCompletionSource<JToken>();
            if (!_receiverTcs.TryAdd(id, tcs)) return null;

            // Initialize the script.
            var run = Script.Replace("$id", id).Replace("$script", script).Replace("$token", _token);
            await _context.RunAsync(() => EvaluateJavascript(run, tcs));
            return await tcs.Task;
        }

        #endregion

        #region Script

        private const string Script = @"(function _$token() {
          if (document.readyState === 'loading') return document.addEventListener('DOMContentLoaded', _$token);
          setTimeout(() => Promise.resolve($script).then(r => $token.resolve($id, JSON.stringify(r)), () => $token.reject($id)), 0);
        })();";

        #endregion
    }
}