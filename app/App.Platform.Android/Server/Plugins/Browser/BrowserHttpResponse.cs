using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using Android.Webkit;

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

        public BrowserHttpResponse(byte[] buffer, HttpContent content, HttpResponseMessage response)
        {
            _buffer = buffer;
            _contentEncoding = content.Headers.ContentEncoding?.FirstOrDefault();
            _contentType = content.Headers.ContentType?.MediaType ?? "application/octet-stream";
            _headers = response.Headers.ToDictionary(x => x.Key, x => string.Join(",", x.Value), StringComparer.InvariantCultureIgnoreCase);
            _statusCode = response.StatusCode;
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