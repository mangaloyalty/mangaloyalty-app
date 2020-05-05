using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Android.Webkit;
using App.Platform.Android.Shared;
using Newtonsoft.Json.Linq;

namespace App.Platform.Android.Server.Plugins.Browser
{
    public class BrowserView
    {
        private readonly Controller _controller;
        private readonly WebView _view;
        private readonly BrowserViewClient _viewClient;
        private readonly ViewCallback _viewScript;

        #region Abstracts

        private static string GetDesktopAgent(string userAgent)
        {
            var withoutAndroid = Regex.Replace(userAgent, @"\(Linux;(.+?)\)", "(X11; Linux x86_64)");
            var withoutMobile = withoutAndroid.Replace("Mobile Safari", "Safari");
            var withoutVersion = Regex.Replace(withoutMobile, @"Version/[0-9\.]+\s+?", "");
            return withoutVersion;
        }

        private void Initialize()
        {
            _view.ClearCache(true);
            _view.Settings.JavaScriptEnabled = true;
            _view.Settings.MixedContentMode = MixedContentHandling.CompatibilityMode;
            _view.Settings.UserAgentString = GetDesktopAgent(_view.Settings.UserAgentString);
            _view.SetWebViewClient(_viewClient);
            CookieManager.Instance.SetAcceptCookie(true);
        }

        #endregion

        #region Constructor
        
        private BrowserView(Controller controller, string viewId)
        {
            _controller = controller;
            _view = new WebView(controller);
            _viewClient = new BrowserViewClient(controller, viewId);
            _viewScript = new ViewCallback(controller, _view);
            Initialize();
        }

        public static async Task<BrowserView> CreateAsync(Controller controller, string viewId)
        {
            return await controller.RunAsync(() => new BrowserView(controller, viewId));
        }

        #endregion

        #region Methods

        public async Task DestroyAsync()
        {
            await _controller.RunAsync(() =>
            {
                _view.Destroy();
                _viewClient.Dispose();
                _viewScript.Dispose();
                _view.Dispose();
            });
        }

        public async Task<JToken> EvaluateAsync(string invoke)
        {
            return await _viewScript.EvaluateAsync($"({invoke})()");
        }

        public async Task NavigateAsync(string url)
        {
            var task = WaitForNavigateAsync();
            await _controller.RunAsync(() => _view.LoadUrl(url));
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