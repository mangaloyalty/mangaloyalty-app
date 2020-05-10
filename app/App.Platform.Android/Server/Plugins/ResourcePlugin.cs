using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Android.Content;
using App.Core.Server;
using App.Core.Server.Models;
using App.Core.Shared.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace App.Platform.Android.Server.Plugins
{
    public class ResourcePlugin : IResourcePlugin
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

        public ResourcePlugin(Context context)
        {
            _context = context;
        }

        #endregion

        #region Implementation of IResourcePlugin

        public Task<bool> MoveAsync(ResourceMoveDataModel model)
        {
            try
            {
                var absoluteFromPath = GetAbsolutePath(model.AbsoluteFromPath);
                var absoluteToPath = GetAbsolutePath(model.AbsoluteToPath);
                Directory.Move(absoluteFromPath, absoluteToPath);
                return Task.FromResult(true);
            }
            catch (DirectoryNotFoundException)
            {
                return Task.FromResult(false);
            }
        }

        public Task<IEnumerable<string>> ReaddirAsync(ResourceDataModel model)
        {
            var absolutePath = GetAbsolutePath(model.AbsolutePath);
            Directory.CreateDirectory(absolutePath);
            return Task.FromResult(Directory.GetFileSystemEntries(absolutePath).Select(Path.GetFileName));
        }

        public async Task<string> ReadFileAsync(ResourceDataModel model)
        {
            try
            {
                var absolutePath = GetAbsolutePath(model.AbsolutePath);
                var buffer = await File.ReadAllBytesAsync(absolutePath);
                return Convert.ToBase64String(buffer);
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }

        public async Task<JToken> ReadJsonAsync(ResourceDataModel model)
        {
            try
            {
                var absolutePath = GetAbsolutePath(model.AbsolutePath);
                var text = await File.ReadAllTextAsync(absolutePath);
                return text.ParseJson();
            }
            catch (FileNotFoundException)
            {
                return null;
            }
        }

        public Task RemoveAsync(ResourceDataModel model)
        {
            try
            {
                Directory.Delete(GetAbsolutePath(model.AbsolutePath), true);
                return Task.CompletedTask;
            }
            catch (DirectoryNotFoundException)
            {
                File.Delete(GetAbsolutePath(model.AbsolutePath));
                return Task.CompletedTask;
            }
        }

        public async Task WriteFileAsync(ResourceWriteFileDataModel model)
        {
            var absolutePath = GetAbsolutePath(model.AbsolutePath);
            var buffer = Convert.FromBase64String(model.Buffer);
            Directory.CreateDirectory(Path.GetDirectoryName(absolutePath) ?? model.AbsolutePath);
            await File.WriteAllBytesAsync(absolutePath, buffer);
        }

        public async Task WriteJsonAsync(ResourceWriteJsonDataModel model)
        {
            var absolutePath = GetAbsolutePath(model.AbsolutePath);
            Directory.CreateDirectory(Path.GetDirectoryName(absolutePath) ?? model.AbsolutePath);
            await File.WriteAllTextAsync(absolutePath, model.Value.ToString(Formatting.Indented));
        }

        #endregion
    }
}