using Android.Webkit;
using App.Core.Interfaces;
using App.Core.Models;

namespace App.Platform.Android.Clients
{
    public sealed class ViewClient : IClient
    {
        private readonly WebView _view;
        
        #region Constructor

        public ViewClient(WebView view)
        {
            _view = view;
        }

        #endregion

        #region Implementation of IClient

        public void Submit(string functionName, SubmitDataModel model)
        {
            _view.Post(() => _view.EvaluateJavascript($"{functionName}({model.InvokeData});", null));
        }

        #endregion
    }
}