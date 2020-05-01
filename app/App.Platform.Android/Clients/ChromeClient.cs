using System;
using Android.Webkit;
using App.Core;

namespace App.Platform.Android.Clients
{
    public sealed class ChromeClient : WebChromeClient
    {
        private readonly Bridge _bridge;

        #region Abstracts

        private bool ProcessRequest(string message)
        {
            _bridge.ProcessRequest(message.Substring(4));
            return false;
        }

        #endregion

        #region Constructor

        public ChromeClient(Bridge bridge)
        {
            _bridge = bridge;
        }

        #endregion
        
        #region Overrides of WebChromeClient

        public override bool OnConsoleMessage(ConsoleMessage consoleMessage)
        {
            if (consoleMessage.Message().StartsWith("oni:")) return ProcessRequest(consoleMessage.Message());
            Console.WriteLine($"${nameof(ChromeClient)}: ${consoleMessage.Message()} (${consoleMessage.LineNumber()})");
            return base.OnConsoleMessage(consoleMessage);
        }

        #endregion
    }
}