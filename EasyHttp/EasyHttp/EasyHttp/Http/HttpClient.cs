#region License

// Distributed under the BSD License
// =================================
// Copyright (c) 2010, Hadi Hariri
// All rights reserved.
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//     * Redistributions of source code must retain the above copyright
//       notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
//     * Neither the name of Hadi Hariri nor the
//       names of its contributors may be used to endorse or promote products
//       derived from this software without specific prior written permission.
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// =============================================================
// 
// Parts of this Software use JsonFX Serialization Library which is distributed under the MIT License:
// Distributed under the terms of an MIT-style license:
// The MIT License
// Copyright (c) 2006-2009 Stephen M. McKamey
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
#endregion

namespace EasyHttp.Http
{
    using System.Collections.Generic;
    using System.Security.Cryptography.X509Certificates;

    using EasyHttp.Configuration;
    using EasyHttp.Contracts;
    using EasyHttp.Exceptions;
    using EasyHttp.Infrastructure;

    /// <summary>
    /// Client for sending Http request and Http responses.
    /// </summary>
    public class HttpClient
    {
        private readonly string baseUri;

        private readonly IDecoder decoder;

        private readonly IEncoder encoder;

        private readonly IUriComposer uriComposer;

        /// <summary>
        /// Client for sending Http request and Http responses.
        /// </summary>
        public HttpClient()
            : this(new DefaultEncoderDecoderConfiguration(), new UriComposer())
        {
        }

        /// <summary>
        /// Client for sending Http request and Http responses.
        /// </summary>
        /// <param name="encoderDecoderConfiguration">Configuration for getting encoder and decoder.</param>
        /// <param name="composer">Composer for generating URI.</param>
        public HttpClient(IEncoderDecoderConfiguration encoderDecoderConfiguration, IUriComposer composer)
        {
            this.encoder = encoderDecoderConfiguration.GetEncoder();
            this.decoder = encoderDecoderConfiguration.GetDecoder();
            this.uriComposer = composer;

            this.Request = new HttpRequest(this.encoder);
        }

        /// <summary>
        /// Client for sending Http request and Http responses. 
        /// </summary>
        /// <param name="baseUri">URI to which  the Http requests will be directed.</param>
        public HttpClient(string baseUri)
            : this(new DefaultEncoderDecoderConfiguration(), new UriComposer())
        {
            this.baseUri = baseUri;
        }

        /// <summary>
        /// Is logging enabled.
        /// </summary>
        public bool LoggingEnabled { get; set; }

        /// <summary>
        /// Should exception be thrown from Http error.
        /// </summary>
        public bool ThrowExceptionOnHttpError { get; set; }

        /// <summary>
        /// Is the response a stream?
        /// </summary>
        public bool StreamResponse { get; set; }

        /// <summary>
        /// Http response.
        /// </summary>
        public HttpResponse Response { get; private set; }

        /// <summary>
        /// Http request.
        /// </summary>
        public HttpRequest Request { get; private set; }

        /// <summary>
        /// Http request for getting a file.
        /// </summary>
        /// <param name="uri">Address to which the request is sent.</param>
        /// <param name="filename">The name of the file that should be recived.</param>
        /// <returns>Http response.</returns>
        public HttpResponse GetAsFile(string uri, string filename)
        {
            this.InitRequest(uri, HttpMethod.GET, null);
            return this.ProcessRequest(filename);
        }

        /// <summary>
        /// Http get request.
        /// </summary>
        /// <param name="uri">Address to which the request is sent.</param>
        /// <param name="query">Request query.</param>
        /// <returns>Http response.</returns>
        public HttpResponse Get(string uri, object query = null)
        {
            this.InitRequest(uri, HttpMethod.GET, query);
            return this.ProcessRequest();
        }

        /// <summary>
        /// Http options request.
        /// </summary>
        /// <param name="uri">Address to which the request is sent.</param>
        /// <returns>Http response.</returns>
        public HttpResponse Options(string uri)
        {
            this.InitRequest(uri, HttpMethod.OPTIONS, null);
            return this.ProcessRequest();
        }

        /// <summary>
        /// Http post request.
        /// </summary>
        /// <param name="uri">Address to which the request is sent.</param>
        /// <param name="data">Posted data.</param>
        /// <param name="contentType">Content type of posted data.</param>
        /// <param name="query">request query.</param>
        /// <returns>Http response.</returns>
        public HttpResponse Post(string uri, object data, string contentType, object query = null)
        {
            // return null;
            this.InitRequest(uri, HttpMethod.POST, query);
            this.InitData(data, contentType);
            return this.ProcessRequest();
        }

        /// <summary>
        /// Http patch request.
        /// </summary>
        /// <param name="uri">Address to which the request is sent.</param>
        /// <param name="data">Data sent.</param>
        /// <param name="contentType">Content type of posted data.</param>
        /// <param name="query">request query.</param>
        /// <returns>Http response.</returns>
        public HttpResponse Patch(string uri, object data, string contentType, object query = null)
        {
            this.InitRequest(uri, HttpMethod.PATCH, query);
            this.InitData(data, contentType);
            return this.ProcessRequest();
        }

        /// <summary>
        /// Http post request with files.
        /// </summary>
        /// <param name="uri">Address to which the request is sent.</param>
        /// <param name="formData">Data sent.</param>
        /// <param name="files">Files sent.</param>
        /// <param name="query">Request query.</param>
        /// <returns>Http response.</returns>
        public HttpResponse Post(
            string uri,
            IDictionary<string, object> formData,
            IList<FileData> files,
            object query = null)
        {
            this.InitRequest(uri, HttpMethod.POST, query);
            this.Request.MultiPartFormData = formData;
            this.Request.MultiPartFileData = files;
            this.Request.KeepAlive = true;
            return this.ProcessRequest();
        }

        /// <summary>
        /// Http put request.
        /// </summary>
        /// <param name="uri">Address to which the request is sent.</param>
        /// <param name="data">Data sent.</param>
        /// <param name="contentType">Content type of the data.</param>
        /// <param name="query">Request query.</param>
        /// <returns>Http response.</returns>
        public HttpResponse Put(string uri, object data, string contentType, object query = null)
        {
            this.InitRequest(uri, HttpMethod.PUT, query);
            this.InitData(data, contentType);
            return this.ProcessRequest();
        }

        /// <summary>
        /// Http Delete request.
        /// </summary>
        /// <param name="uri">Address to which the request is sent.</param>
        /// <param name="query">Request query.</param>
        /// <returns>Http response.</returns>
        public HttpResponse Delete(string uri, object query = null)
        {
            this.InitRequest(uri, HttpMethod.DELETE, query);
            return this.ProcessRequest();
        }

        /// <summary>
        /// Http head reqest.
        /// </summary>
        /// <param name="uri">Address to which the request is sent.</param>
        /// <param name="query">Request query.</param>
        /// <returns>Http response.</returns>
        public HttpResponse Head(string uri, object query = null)
        {
            this.InitRequest(uri, HttpMethod.HEAD, query);
            return this.ProcessRequest();
        }

        /// <summary>
        /// Http put file request.
        /// </summary>
        /// <param name="uri">Address to which the request is sent.</param>
        /// <param name="filename">Name of the file for request.</param>
        /// <param name="contentType">Content type of the file.</param>
        /// <returns>Http request.</returns>
        public HttpResponse PutFile(string uri, string filename, string contentType)
        {
            this.InitRequest(uri, HttpMethod.PUT, null);
            this.Request.ContentType = contentType;
            this.Request.PutFilename = filename;
            this.Request.Expect = true;
            this.Request.KeepAlive = true;
            return this.ProcessRequest();
        }

        /// <summary>
        /// Method for adding client certificates.
        /// </summary>
        /// <param name="certificates">Certificates for adding.</param>
        public void AddClientCertificates(X509CertificateCollection certificates)
        {
            if (certificates == null || certificates.Count == 0)
            {
                return;
            }

            this.Request.ClientCertificates.AddRange(certificates);
        }

        /// <summary>
        /// Initialize the Http request. 
        /// </summary>
        /// <param name="uri">Address to which the request is sent.</param>
        /// <param name="method">Http method for sending the request.</param>
        /// <param name="query">Reqest query.</param>
        private void InitRequest(string uri, HttpMethod method, object query)
        {
            this.Request.Uri = this.uriComposer.Compose(this.baseUri, uri, query, this.Request.ParametersAsSegments);
            this.Request.Data = null;
            this.Request.PutFilename = string.Empty;
            this.Request.Expect = false;
            this.Request.KeepAlive = true;
            this.Request.MultiPartFormData = null;
            this.Request.MultiPartFileData = null;
            this.Request.ContentEncoding = null;
            this.Request.Method = method;
        }

        /// <summary>
        /// Initialize request data.
        /// </summary>
        /// <param name="data">Data that is initialized.</param>
        /// <param name="contentType">Content type of the data.</param>
        private void InitData(object data, string contentType)
        {
            if (data != null)
            {
                this.Request.ContentType = contentType;
                this.Request.Data = data;
            }
        }

        /// <summary>
        /// Method for proccesing an Http request.
        /// </summary>
        /// <param name="filename">File name.</param>
        /// <returns>Http response.</returns>
        private HttpResponse ProcessRequest(string filename = "")
        {
            var httpWebRequest = this.Request.PrepareRequest();

            this.Response = new HttpResponse(this.decoder);

            this.Response.GetResponse(httpWebRequest, filename, this.StreamResponse);

            if (this.ThrowExceptionOnHttpError && this.IsHttpError())
            {
                throw new HttpException(this.Response.StatusCode, this.Response.StatusDescription);
            }

            return this.Response;
        }

        /// <summary>
        /// Method for checking is the response status code is an error.
        /// </summary>
        /// <returns>True if there is an error. Otherwise returns false.</returns>
        private bool IsHttpError()
        {
            var num = (int)this.Response.StatusCode / 100;

            return num == 4 || num == 5;
        }
    }
}