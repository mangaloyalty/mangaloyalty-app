using System;
using App.Core;
using WebKit;

namespace App.Platform.iOS.Clients
{
    public class WebClient : WKScriptMessageHandler
    {
        private Bridge _bridge;

        #region Methods

        public void UseBridge(Bridge bridge)
        {
            _bridge = bridge;
        }

        #endregion

        #region Overrides of UIViewController

        public override void DidReceiveScriptMessage(WKUserContentController userContentController, WKScriptMessage message)
        {
            var body = message.Body.ToString();
            var text = body.Substring(2);
            if (text.StartsWith("oni:")) _bridge.ProcessRequest(text.Substring(4));
            else Console.WriteLine(body);
        }

        #endregion
    }
}