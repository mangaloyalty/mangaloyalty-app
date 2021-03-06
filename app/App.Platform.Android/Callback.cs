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
        public void ReceiveReject(string id, string message)
        {
            if (!_receiverTcs.TryRemove(id, out var receiverTcs)) return;
            receiverTcs.TrySetException(new Exception(message));
        }

        #endregion

        #region Implementation of ICallback

        public async Task<JToken> EvaluateAsync(string script)
        {
            // Initialize the identifier.
            var id = Interlocked.Increment(ref _previousId).ToString();
            var receiverTcs = new TimeoutTaskCompletionSource<JToken>();
            if (!_receiverTcs.TryAdd(id, receiverTcs)) return null;

            // Initialize the script.
            var tracker = Script.Replace("$i", id).Replace("$s", script).Replace("$t", _token);
            await _context.RunAsync(() => _view.EvaluateJavascript(tracker, null));
            return await receiverTcs.Task;
        }

        #endregion

        #region Script

        private const string Script = @"(function _$t() {
          if (document.readyState === 'loading') return document.addEventListener('DOMContentLoaded', _$t);
          setTimeout(() => Promise.resolve($s).then(v => $t.resolve($i, JSON.stringify(v)), e => $t.reject($i, String(e && e.stack || e))), 0);
        })();";

        #endregion
    }
}