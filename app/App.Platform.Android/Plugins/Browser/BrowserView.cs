using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Android.Webkit;
using Newtonsoft.Json.Linq;

namespace App.Platform.Android.Plugins.Browser
{
    public class BrowserView
    {
        private readonly WebView _view;
        private readonly BrowserViewClient _viewClient;
        private readonly BrowserViewScript _viewScript;

        #region Abstracts

        private static string GetDesktopAgent(string userAgent)
        {
            var withoutAndroid = Regex.Replace(userAgent, @"\(Linux;(.+?)\)", "(X11; Linux x86_64)");
            var withoutMobile = withoutAndroid.Replace("Mobile Safari", "Safari");
            var withoutVersion = Regex.Replace(withoutMobile, @"Version/[0-9\.]+\s+?", "");
            return withoutVersion;
        }

        #endregion

        #region Constructor
        
        public BrowserView(MainActivity activity, string viewId)
        {
            _view = new WebView(activity);
            _viewClient = new BrowserViewClient(activity, viewId);
            _viewScript = new BrowserViewScript(_view);
            _view.ClearCache(true);
            _view.Settings.JavaScriptEnabled = true;
            _view.Settings.MixedContentMode = MixedContentHandling.CompatibilityMode;
            _view.Settings.UserAgentString = GetDesktopAgent(_view.Settings.UserAgentString);
            _view.SetWebViewClient(_viewClient);
        }

        #endregion

        #region Methods
        
        public Task DestroyAsync()
        {
            _viewScript.Dispose();
            _view.Destroy();
            _view.Dispose();
            return Task.CompletedTask;
        }

        public async Task<JToken> EvaluateAsync(string invoke)
        {
            return await _viewScript.EvaluateAsync(invoke);
        }

        public async Task NavigateAsync(string url)
        {
            var task = WaitForNavigateAsync();
            _view.LoadUrl(url);
            await task;
        }

        public async Task<byte[]> ResponseAsync(string url)
        {
            return await _viewClient.ResponseAsync(url);
        }

        public async Task WaitForNavigateAsync()
        {
            await _viewClient.WaitForNavigateAsync();
        }

        #endregion
    }
}