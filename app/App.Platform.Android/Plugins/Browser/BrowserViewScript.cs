using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Android.App;
using Android.Webkit;
using App.Platform.Android.Utilities;
using App.Platform.Android.Utilities.Extensions;
using Java.Interop;
using Newtonsoft.Json.Linq;

namespace App.Platform.Android.Plugins.Browser
{
    public class BrowserViewScript : Java.Lang.Object
    {
        private readonly Activity _activity;
        private readonly ConcurrentDictionary<string, TaskCompletionSource<JToken>> _callbacks;
        private readonly string _token;
        private readonly WebView _view;
        private int _previousId;

        #region Constructor

        public BrowserViewScript(Activity activity, WebView view)
        {
            _activity = activity;
            _callbacks = new ConcurrentDictionary<string, TaskCompletionSource<JToken>>();
            _token = char.ConvertFromUtf32(new Random().Next(97, 122)) + new Random().Next(100, 999999);
            (_view = view).AddJavascriptInterface(this, _token);
        }
        
        #endregion

        #region Methods

        public async Task<JToken> EvaluateAsync(string invoke)
        {
            // Initialize the identifier.
            var id = Interlocked.Increment(ref _previousId).ToString();
            var tcs = new TimeoutTaskCompletionSource<JToken>(30);
            if (!_callbacks.TryAdd(id, tcs)) return null;

            // Initialize the script.
            var eval = Script.Replace("$i", id).Replace("$s", invoke).Replace("$t", _token);
            await _activity.InvokeAsync(() => _view.EvaluateJavascript(eval, null));
            return await tcs.Task;
        }

        [Export("resolve")]
        [JavascriptInterface]
        public void ReceiveResolve(string id, string value)
        {
            if (!_callbacks.TryGetValue(id, out var resolver)) return;
            resolver.TrySetResult(value != null ? JToken.Parse(value) : null);
        }

        [Export("reject")]
        [JavascriptInterface]
        public void ReceiveReject(string id)
        {
            if (!_callbacks.TryGetValue(id, out var resolver)) return;
            resolver.TrySetCanceled();
        }

        #endregion

        #region Script

        private const string Script = @"Promise.resolve(($s)()).then(x => {
            $t.resolve($i, JSON.stringify(x));
        }, () => {
            $t.reject($i);
        });";

        #endregion
    }
}