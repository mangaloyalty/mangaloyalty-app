using Android.Webkit;
using App.Core.Interfaces;
using App.Core.Models;

namespace App.Platform.Android.Clients
{
    public sealed class BridgeClient : IClient
    {
        private readonly WebView _webView;
        
        #region Constructor

        public BridgeClient(WebView webView)
        {
            _webView = webView;
        }

        #endregion

        #region Implementation of IClient

        public void Submit(string functionName, SubmitDataModel model)
        {
            _webView.Post(() => _webView.EvaluateJavascript($"{functionName}({model.InvokeData});", null));
        }

        #endregion
    }
}