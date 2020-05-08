using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Android.Content;
using Android.Webkit;
using Newtonsoft.Json.Linq;

namespace App.Platform.Android.Server.Plugins.Browser
{
    public class BrowserView
    {
        private readonly Context _context;
        private readonly WebView _view;
        private readonly BrowserViewClient _viewClient;
        private readonly Callback _viewScript;

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

        private BrowserView(Context context, ServerCore core, string viewId)
        {
            _context = context;
            _view = new WebView(context);
            _viewClient = new BrowserViewClient(core, viewId);
            _viewScript = new Callback(context, _view);
            Initialize();
        }

        public static async Task<BrowserView> CreateAsync(Context context, ServerCore core, string viewId)
        {
            return await context.RunAsync(() => new BrowserView(context, core, viewId));
        }

        #endregion

        #region Methods

        public async Task DestroyAsync()
        {
            await _context.RunAsync(() =>
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
            await _context.RunAsync(() => _view.LoadUrl(url));
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