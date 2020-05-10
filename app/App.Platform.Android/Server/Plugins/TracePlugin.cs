using System;
using System.IO;
using System.Threading.Tasks;
using Android.Content;
using App.Core.Server;
using App.Core.Server.Models;

namespace App.Platform.Android.Server.Plugins
{
    public class TracePlugin : ITracePlugin
    {
        private readonly Context _context;

        #region Abstracts

        private string GetAbsolutePath(string absolutePath)
        {
            var basePath = _context.GetExternalFilesDir(null).AbsolutePath;
            var relativePath = absolutePath.Substring(14);
            return Path.Combine(basePath, relativePath);
        }

        #endregion

        #region Constructor

        public TracePlugin(Context context)
        {
            _context = context;
        }

        #endregion

        #region Implementation of ITracePlugin

        public async Task InfoAsync(TraceDataModel model)
        {
            var absolutePath = GetAbsolutePath(model.AbsolutePath);
            Directory.CreateDirectory(Path.GetDirectoryName(absolutePath) ?? model.AbsolutePath);
            await using var fs = File.Open(absolutePath, FileMode.Append);
            await using var sw = new StreamWriter(fs);
            await sw.WriteLineAsync($"{DateTime.Now:s}: {model.Message}");
            await sw.FlushAsync();
        }

        #endregion
    }
}