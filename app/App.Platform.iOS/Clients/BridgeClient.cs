using App.Core.Interfaces;
using App.Core.Models;
using WebKit;

namespace App.Platform.iOS.Clients
{
    public sealed class BridgeClient : IClient
    {
        private readonly WKWebView _webView;

        #region Constructor

        public BridgeClient(WKWebView webView)
        {
            _webView = webView;
        }

        #endregion

        #region Implementation of IClient

        public void Submit(string functionName, SubmitDataModel model)
        {
            _webView.InvokeOnMainThread(() =>
            {
                _webView.EvaluateJavaScript($"{functionName}({model.InvokeData});", null);
            });
        }

        #endregion
    }
}