using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using Android.Webkit;
using App.Platform.Android.Server.Plugins.Browser.Extensions;

namespace App.Platform.Android.Server.Plugins.Browser
{
    public class BrowserHttpResponse
    {
        private readonly byte[] _buffer;
        private readonly string _contentEncoding;
        private readonly string _contentType;
        private readonly IDictionary<string, string> _headers;
        private readonly HttpStatusCode _statusCode;

        #region Constructor

        public BrowserHttpResponse(byte[] buffer, string contentEncoding, string contentType, IDictionary<string, string> headers, HttpStatusCode statusCode)
        {
            _buffer = buffer;
            _contentEncoding = contentEncoding;
            _contentType = contentType;
            _headers = headers;
            _statusCode = statusCode;
        }

        public static BrowserHttpResponse Create(byte[] buffer, HttpContent content, HttpResponseMessage message)
        {
            var contentEncoding = content.Headers.ContentEncoding.FirstOrDefault();
            var contentType = content.Headers.ContentType.MediaType;
            var headers = message.Headers.ToDictionary("Cache-Control", "Expires", "ETag", "Last-Modified", "Pragma");
            var statusCode = message.StatusCode;
            return new BrowserHttpResponse(buffer, contentEncoding, contentType, headers, statusCode);
        }
        
        #endregion

        #region Methods

        public WebResourceResponse ToResourceResponse()
        {
            var statusCode = (int) _statusCode;
            var statusDescription = _statusCode.ToString();
            var stream = new MemoryStream(_buffer);
            return new WebResourceResponse(_contentType, _contentEncoding, statusCode, statusDescription, _headers, stream);
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