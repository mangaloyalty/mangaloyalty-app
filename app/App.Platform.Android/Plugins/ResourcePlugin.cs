﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using App.Core.Models.Plugins;
using App.Core.Plugins;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace App.Platform.Android.Plugins
{
    public class ResourcePlugin : IResourcePlugin
    {
        #region Abstracts

        private static string GetAbsolutePath(string absolutePath)
        {
            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var relativePath = absolutePath.TrimStart('/');
            return Path.Combine(basePath, relativePath);
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
                return JToken.Parse(text);
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