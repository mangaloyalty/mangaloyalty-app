using System;
using System.IO;
using System.Threading.Tasks;
using App.Core.Server;
using App.Core.Server.Models;

namespace App.Platform.Android.Server.Plugins
{
    public class TracePlugin : ITracePlugin
    {
        #region Abstracts

        private static string GetAbsolutePath(string absolutePath)
        {
            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var relativePath = absolutePath.TrimStart('/');
            return Path.Combine(basePath, relativePath);
        }

        #endregion

        #region Implementation of ITracePlugin

        public async Task InfoAsync(TraceDataModel model)
        {
            await using var fs = File.Open(GetAbsolutePath(model.AbsolutePath), FileMode.Append);
            await using var sw = new StreamWriter(fs);
            await sw.WriteLineAsync($"{DateTime.Now:s}: {model.Message}");
            await sw.FlushAsync();
        }

        #endregion
    }
}