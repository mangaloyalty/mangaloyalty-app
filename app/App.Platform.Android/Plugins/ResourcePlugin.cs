using System;
using System.IO;
using System.Threading.Tasks;
using App.Core.Models.Plugins;
using App.Core.Plugins;
using Newtonsoft.Json.Linq;

namespace App.Platform.Android.Plugins
{
    public class ResourcePlugin : IResourcePlugin
    {
        private string GetAbsolutePath(string absolutePath)
        {
            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var relativePath = absolutePath.TrimStart('/');
            return Path.Combine(basePath, relativePath);
        }

        public Task<bool> MoveAsync(ResourceMoveDataModel model)
        {
            throw new NotImplementedException();
        }

        public Task<string[]> ReaddirAsync(ResourceDataModel model)
        {
            var absolutePath = GetAbsolutePath(model.AbsolutePath);
            Directory.CreateDirectory(absolutePath);
            return Task.FromResult(Directory.GetFileSystemEntries(absolutePath));
        }

        public async Task<string> ReadFileAsync(ResourceDataModel model)
        {
            try
            {
                var absolutePath = GetAbsolutePath(model.AbsolutePath);
                var bytes = await File.ReadAllBytesAsync(absolutePath);
                return Convert.ToBase64String(bytes);
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
                try
                {
                    File.Delete(GetAbsolutePath(model.AbsolutePath));
                    return Task.CompletedTask;
                }
                catch (FileNotFoundException)
                {
                    return Task.CompletedTask;
                }
            }
        }

        public Task WriteFileAsync(ResourceWriteFileDataModel model)
        {
            throw new NotImplementedException();
        }

        public Task WriteJsonAsync(ResourceWriteJsonDataModel model)
        {
            throw new NotImplementedException();
        }
    }
}