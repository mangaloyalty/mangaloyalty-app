﻿using App.Core.Client;
using App.Platform.iOS.Clients;

namespace App.Platform.iOS.Plugins
{
    public class ShellPlugin : IShellPlugin
    {
        private readonly ViewClient _view;

        #region Constructor

        public ShellPlugin(ViewClient view)
        {
            _view = view;
        }

        #endregion

        #region Implementation of IShellPlugin

        public void HideSplashScreen()
        {
            _view.HideSplashScreen();
        }
        
        #endregion
    }
}