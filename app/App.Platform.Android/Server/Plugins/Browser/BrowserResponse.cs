using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Android.Webkit;

namespace App.Platform.Android.Server.Plugins.Browser
{
    public class BrowserResponse
    {
        private readonly byte[] _buffer;
        private readonly HttpWebResponse _response;

        #region Constructor

        private BrowserResponse(byte[] buffer, HttpWebResponse response)
        {
            _buffer = buffer;
            _response = response;
        }
        
        public static async Task<BrowserResponse> CreateAsync(HttpWebResponse response)
        {
            await using var memoryStream = new MemoryStream();
            await using var responseStream = response.GetResponseStream();
            await (responseStream?.CopyToAsync(memoryStream) ?? Task.CompletedTask);
            return new BrowserResponse(memoryStream.ToArray(), response);
        }

        #endregion

        #region Methods

        public WebResourceResponse ToWebViewResponse()
        {
            var contentEncoding = _response.ContentEncoding;
            var contentType = Regex.Replace(_response.ContentType, @"\s*;(.*)$", string.Empty);
            var headers = _response.Headers.AllKeys.ToDictionary(x => x, x => _response.Headers[x]);
            var statusCode = (int) _response.StatusCode;
            var statusDescription = _response.StatusDescription;
            var stream = new MemoryStream(_buffer);
            return new WebResourceResponse(contentType, contentEncoding, statusCode, statusDescription, headers, stream);
        }

        #endregion

        #region Properties

        public byte[] Buffer
        {
            get { return _buffer; }
        }

        #endregion
    }
}