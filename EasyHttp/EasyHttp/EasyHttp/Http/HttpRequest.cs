#region License

// Distributed under the BSD License
// YouTrackSharp Copyright (c) 2010-2012, Hadi Hariri and Contributors
// All rights reserved.
//  Redistribution and use in source and binary forms, with or without
//  modification, are permitted provided that the following conditions are met:
//      * Redistributions of source code must retain the above copyright
//         notice, this list of conditions and the following disclaimer.
//      * Redistributions in binary form must reproduce the above copyright
//         notice, this list of conditions and the following disclaimer in the
//         documentation and/or other materials provided with the distribution.
//      * Neither the name of Hadi Hariri nor the
//         names of its contributors may be used to endorse or promote products
//         derived from this software without specific prior written permission.
//   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
//   "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED
//   TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A 
//   PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL 
//   <COPYRIGHTHOLDER> BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
//   SPECIAL,EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
//   LIMITED  TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
//   DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND  ON ANY
//   THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
//   (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
//   THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
#endregion

namespace EasyHttp.Http
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Cache;
    using System.Net.Security;
    using System.Reflection;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;

    using EasyHttp.Codecs;
    using EasyHttp.Contracts;
    using EasyHttp.Infrastructure;

    /// <summary>
    /// Http request.
    /// </summary>
    public class HttpRequest
    {
        private readonly IEncoder encoder;

        private HttpRequestCachePolicy cachePolicy;

        private string password;

        private string username;

        private CookieContainer cookieContainer;

        private HttpWebRequest httpWebRequest;

        /// <summary>
        /// Http request.
        /// </summary>
        /// <param name="encoder">Encoder for encoding  the request.</param>
        public HttpRequest(IEncoder encoder)
        {
            this.RawHeaders = new Dictionary<string, object>();

            this.ClientCertificates = new X509CertificateCollection();

            this.UserAgent = string.Format(
                "EasyHttp HttpClient v{0}",
                Assembly.GetAssembly(typeof(HttpClient)).GetName().Version);

            this.Accept = string.Join(
                ";",
                HttpContentTypes.TextHtml,
                HttpContentTypes.ApplicationXml,
                HttpContentTypes.ApplicationJson);
            this.encoder = encoder;

            this.Timeout = 100000; // http://msdn.microsoft.com/en-us/library/system.net.httpwebrequest.timeout.aspx

            this.AllowAutoRedirect = true;
        }

        /// <summary>
        /// Mime types that will be accepted as response.
        /// </summary>
        public string Accept { get; set; }

        public string AcceptCharSet { get; set; }

        /// <summary>
        /// Type of encoding that will be accept.
        /// </summary>
        public string AcceptEncoding { get; set; }

        /// <summary>
        /// Expected response language.
        /// </summary>
        public string AcceptLanguage { get; set; }

        /// <summary>
        /// Should the connection be close after the response is received.
        /// </summary>
        public bool KeepAlive { get; set; }

        /// <summary>
        /// Collection of client certificates.
        /// </summary>
        public X509CertificateCollection ClientCertificates { get; set; }

        /// <summary>
        /// Content length.
        /// </summary>
        public string ContentLength { get; private set; }

        /// <summary>
        /// Content type.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Content encoding.
        /// </summary>
        public string ContentEncoding { get; set; }

        /// <summary>
        /// Collection of cookies.
        /// </summary>
        public CookieCollection Cookies { get; set; }

        /// <summary>
        /// Date of sending request.
        /// </summary>
        public DateTime Date { get; set; }

        public bool Expect { get; set; }

        /// <summary>
        /// Request sender.
        /// </summary>
        public string From { get; set; }

        /// <summary>
        /// Request host.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// If none of the entity tags match, or if "*" is given and no current entity exists, 
        /// the server MUST NOT perform the requested method, 
        /// and MUST return a 412 (Precondition Failed) response.
        /// </summary>
        public string IfMatch { get; set; }

        /// <summary>
        /// If the requested variant has not been modified since the time specified in this field,
        /// an entity will not be returned from the server; instead, a 304 (not modified) 
        /// response will be returned without any message-body.
        /// </summary>
        public DateTime IfModifiedSince { get; set; }

        /// <summary>
        /// If the entity is unchanged, send the part(s) that are missing; otherwise, send the entire new entity.
        /// </summary>
        public string IfRange { get; set; }

        /// <summary>
        /// The Max-Forwards value is a decimal integer indicating 
        /// the remaining number of times this request message may be forwarded.
        /// </summary>
        public int MaxForwards { get; set; }

        /// <summary>
        /// Allows the client to specify, for the server's benefit,
        /// the address (URI) of the resource from which the Request-URI was obtained
        /// </summary>
        public string Referer { get; set; }

        /// <summary>
        /// Request range.
        /// </summary>
        public int Range { get; set; }

        /// <summary>
        ///  Contains information about the user agent originating the request.
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// Request headers in raw format.
        /// </summary>
        public IDictionary<string, object> RawHeaders { get; private set; }

        /// <summary>
        /// Http Method.
        /// </summary>
        public HttpMethod Method { get; set; }

        /// <summary>
        /// Request data.
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// Address to which the request is sent. 
        /// </summary>
        public string Uri { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PutFilename { get; set; }

        /// <summary>
        /// Data from MyltiPart form that will be send. 
        /// </summary>
        public IDictionary<string, object> MultiPartFormData { get; set; }

        /// <summary>
        /// File data from MultiPart form.
        /// </summary>
        public IList<FileData> MultiPartFileData { get; set; }

        /// <summary>
        /// Time to wait for response before the request is canceled.
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// Are query string parameters represented as segments in the URI.
        /// </summary>
        public bool ParametersAsSegments { get; set; }

        public bool ForceBasicAuth { get; set; }

        /// <summary>
        /// Should cookies are persisted.
        /// </summary>
        public bool PersistCookies { get; set; }

        /// <summary>
        /// Should auto redirect be allowed.
        /// </summary>
        public bool AllowAutoRedirect { get; set; }

        /// <summary>
        /// Method for setting basic authentication. 
        /// </summary>
        /// <param name="username">Authentication username.</param>
        /// <param name="password">Authentication password.</param>
        public void SetBasicAuthentication(string username, string password)
        {
            this.username = username;
            this.password = password;
        }

        /// <summary>
        /// Method for adding additional headers.
        /// </summary>
        /// <param name="header">Name of the header that will be added.</param>
        /// <param name="value">Value of the header that will be added.</param>
        public void AddExtraHeader(string header, object value)
        {
            if (value != null && !this.RawHeaders.ContainsKey(header))
            {
                this.RawHeaders.Add(header, value);
            }
        }

        /// <summary>
        /// Method for preparing request for sending.
        /// </summary>
        /// <returns>Http request ready to be sent.</returns>
        public HttpWebRequest PrepareRequest()
        {
            this.httpWebRequest = (HttpWebRequest)WebRequest.Create(this.Uri);
            this.httpWebRequest.AllowAutoRedirect = this.AllowAutoRedirect;
            this.SetupHeader();

            this.SetupBody();

            return this.httpWebRequest;
        }

        /// <summary>
        /// Method for setting  the cache control to no-cache.
        /// </summary>
        public void SetCacheControlToNoCache()
        {
            this.cachePolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
        }

        /// <summary>
        /// Method for setting the cache control with max age.
        /// </summary>
        /// <param name="maxAge">Maximum age for keeping the cache.</param>
        public void SetCacheControlWithMaxAge(TimeSpan maxAge)
        {
            this.cachePolicy = new HttpRequestCachePolicy(HttpCacheAgeControl.MaxAge, maxAge);
        }

        public void SetCacheControlWithMaxAgeAndMaxStale(TimeSpan maxAge, TimeSpan maxStale)
        {
            this.cachePolicy = new HttpRequestCachePolicy(HttpCacheAgeControl.MaxAgeAndMaxStale, maxAge, maxStale);
        }

        public void SetCacheControlWithMaxAgeAndMinFresh(TimeSpan maxAge, TimeSpan minFresh)
        {
            this.cachePolicy = new HttpRequestCachePolicy(HttpCacheAgeControl.MaxAgeAndMinFresh, maxAge, minFresh);
        }

        /// <summary>
        /// Method for setup the request headers. 
        /// </summary>
        private void SetupHeader()
        {
            if (!this.PersistCookies || this.cookieContainer == null)
            {
                this.cookieContainer = new CookieContainer();
            }

            this.httpWebRequest.CookieContainer = this.cookieContainer;
            this.httpWebRequest.ContentType = this.ContentType;
            this.httpWebRequest.Accept = this.Accept;
            this.httpWebRequest.Method = this.Method.ToString();
            this.httpWebRequest.UserAgent = this.UserAgent;
            this.httpWebRequest.Referer = this.Referer;
            this.httpWebRequest.CachePolicy = this.cachePolicy;
            this.httpWebRequest.KeepAlive = this.KeepAlive;
            this.httpWebRequest.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip
                                                         | DecompressionMethods.None;

            ServicePointManager.Expect100Continue = this.Expect;
            ServicePointManager.ServerCertificateValidationCallback = this.AcceptAllCertifications;

            if (this.Timeout > 0)
            {
                this.httpWebRequest.Timeout = this.Timeout;
            }

            if (this.Cookies != null)
            {
                this.httpWebRequest.CookieContainer.Add(this.Cookies);
            }

            if (this.IfModifiedSince != DateTime.MinValue)
            {
                this.httpWebRequest.IfModifiedSince = this.IfModifiedSince;
            }

            if (this.Date != DateTime.MinValue)
            {
                this.httpWebRequest.Date = this.Date;
            }

            if (!string.IsNullOrEmpty(this.Host))
            {
                this.httpWebRequest.Host = this.Host;
            }

            if (this.MaxForwards != 0)
            {
                this.httpWebRequest.MaximumAutomaticRedirections = this.MaxForwards;
            }

            if (this.Range != 0)
            {
                this.httpWebRequest.AddRange(this.Range);
            }

            this.SetupAuthentication();

            this.AddExtraHeader("From", this.From);
            this.AddExtraHeader("Accept-CharSet", this.AcceptCharSet);
            this.AddExtraHeader("Accept-Encoding", this.AcceptEncoding);
            this.AddExtraHeader("Accept-Language", this.AcceptLanguage);
            this.AddExtraHeader("If-Match", this.IfMatch);
            this.AddExtraHeader("Content-Encoding", this.ContentEncoding);

            foreach (var header in this.RawHeaders)
            {
                this.httpWebRequest.Headers.Add(string.Format("{0}: {1}", header.Key, header.Value));
            }
        }

        private bool AcceptAllCertifications(
            object sender,
            X509Certificate certificate,
            X509Chain chain,
            SslPolicyErrors sslpolicyerrors)
        {
            return true;
        }

        /// <summary>
        /// Method for setting up of the request body. 
        /// </summary>
        private void SetupBody()
        {
            if (this.Data != null)
            {
                this.SetupData();
            }
            else if (!string.IsNullOrEmpty(this.PutFilename))
            {
                this.SetupPutFilename();
            }
            else if (this.MultiPartFormData != null || this.MultiPartFileData != null)
            {
                this.SetupMultiPartBody();
            }
        }

        /// <summary>
        /// Method for setting up the request data.
        /// </summary>
        private void SetupData()
        {
            var bytes = this.encoder.Encode(this.Data, this.ContentType);

            if (bytes.Length > 0)
            {
                this.httpWebRequest.ContentLength = bytes.Length;
            }

            var requestStream = this.httpWebRequest.GetRequestStream();

            requestStream.Write(bytes, 0, bytes.Length);

            requestStream.Close();
        }

        /// <summary>
        /// Method for seeting up PutFileName.
        /// </summary>
        private void SetupPutFilename()
        {
            using (var fileStream = new FileStream(this.PutFilename, FileMode.Open))
            {
                this.httpWebRequest.ContentLength = fileStream.Length;

                var requestStream = this.httpWebRequest.GetRequestStream();

                var buffer = new byte[81982];

                int bytesRead = fileStream.Read(buffer, 0, buffer.Length);
                while (bytesRead > 0)
                {
                    requestStream.Write(buffer, 0, bytesRead);
                    bytesRead = fileStream.Read(buffer, 0, buffer.Length);
                }

                requestStream.Close();
            }
        }

        /// <summary>
        /// Method for setting up multipart form body.
        /// </summary>
        private void SetupMultiPartBody()
        {
            var multiPartStreamer = new MultiPartStreamer(this.MultiPartFormData, this.MultiPartFileData);

            this.httpWebRequest.ContentType = multiPartStreamer.GetContentType();
            var contentLength = multiPartStreamer.GetContentLength();

            if (contentLength > 0)
            {
                this.httpWebRequest.ContentLength = contentLength;
            }

            multiPartStreamer.StreamMultiPart(this.httpWebRequest.GetRequestStream());
        }

        /// <summary>
        /// Method for seeting up client certificates.
        /// </summary>
        private void SetupClientCertificates()
        {
            if (this.ClientCertificates == null || this.ClientCertificates.Count == 0)
            {
                return;
            }

            this.httpWebRequest.ClientCertificates.AddRange(this.ClientCertificates);
        }

        /// <summary>
        /// Method for setting up authentication.
        /// </summary>
        private void SetupAuthentication()
        {
            this.SetupClientCertificates();

            if (this.ForceBasicAuth)
            {
                string authInfo = this.username + ":" + this.password;
                authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                this.httpWebRequest.Headers["Authorization"] = "Basic " + authInfo;
            }
            else
            {
                var networkCredential = new NetworkCredential(this.username, this.password);
                this.httpWebRequest.Credentials = networkCredential;
            }
        }
    }
}