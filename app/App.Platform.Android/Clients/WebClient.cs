using System;
using Android.App;
using Android.Content;
using Android.Webkit;

namespace App.Platform.Android.Clients
{
    public sealed class WebClient : WebViewClient
    {
        private readonly Activity _activity;

        #region Constructor

        public WebClient(Activity activity)
        {
            _activity = activity;
        }

        #endregion

        #region Overrides of WebViewClient

        public override void OnReceivedError(WebView view, IWebResourceRequest request, WebResourceError error)
        {
            Console.WriteLine($"${nameof(WebClient)}: ${request.Url.ToString()} (${error.ErrorCode})");
            base.OnReceivedError(view, request, error);
        }

        public override bool ShouldOverrideUrlLoading(WebView view, IWebResourceRequest request)
        {
            if (URLUtil.IsNetworkUrl(request.Url.ToString())) return false;
            _activity.StartActivity(new Intent(Intent.ActionView, request.Url));
            return true;
        }

        #endregion
    }
}