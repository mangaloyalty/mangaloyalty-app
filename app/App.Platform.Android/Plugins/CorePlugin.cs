﻿using App.Core.Plugins;

namespace App.Platform.Android.Plugins
{
    public sealed class CorePlugin : ICorePlugin
    {
        #region Constructor

        public CorePlugin(MainActivity activity)
        {
            Browser = new BrowserPlugin(activity);
            Resource = new ResourcePlugin();
            Shell = new ShellPlugin(activity);
            Trace = new TracePlugin();
        }

        #endregion

        #region Implementation of ICorePlugin

        public IBrowserPlugin Browser { get; }
        public IResourcePlugin Resource { get; }
        public IShellPlugin Shell { get; }
        public ITracePlugin Trace { get; }

        #endregion
    }
}