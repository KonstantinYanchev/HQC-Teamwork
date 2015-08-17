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
    using System;
    using System.IO;
    using System.Net;
    using System.Text;

    using EasyHttp.Codecs;
    using EasyHttp.Contracts;

    /// <summary>
    /// Http response.
    /// </summary>
    public class HttpResponse
    {
        private readonly IDecoder decoder;

        private HttpWebResponse response;

        /// <summary>
        /// Http response.
        /// </summary>
        /// <param name="decoder">Decoder for http response.</param>
        public HttpResponse(IDecoder decoder)
        {
            this.decoder = decoder;
        }

        /// <summary>
        /// Response character set.
        /// </summary>
        public string CharacterSet { get; private set; }

        /// <summary>
        /// indicates the media type of the entity-body sent to the recipient
        /// </summary>
        public string ContentType { get; private set; }

        /// <summary>
        /// Http response status code.
        /// </summary>
        public HttpStatusCode StatusCode { get; private set; }

        /// <summary>
        /// Http response status description of code.
        /// </summary>
        public string StatusDescription { get; private set; }

        /// <summary>
        /// Collection of cookies.
        /// </summary>
        public CookieCollection Cookies { get; private set; }

        /// <summary>
        /// Conveys the sender's estimate of the amount of time 
        /// since the response was generated at the origin server.
        /// </summary>
        public int Age { get; private set; }

        /// <summary>
        /// Inform the recipient of valid methods
        /// associated with the resource.
        /// </summary>
        public HttpMethod[] Allow { get; private set; }

        /// <summary>
        /// Specify directives that MUST be obeyed by all caching mechanisms along the request/response chain.
        /// </summary>
        public CacheControl CacheControl { get; private set; }

        /// <summary>
        /// Indicates what additional content codings have been applied to the entity-body.
        /// </summary>
        public string ContentEncoding { get; private set; }

        /// <summary>
        /// Describes the natural language(s) of the intended audience for the enclosed entity.
        /// </summary>
        public string ContentLanguage { get; private set; }

        /// <summary>
        ///  The size of the entity-body, in decimal number of OCTETs, sent to the recipient.
        /// </summary>
        public long ContentLength { get; private set; }

        /// <summary>
        /// Used to supply the resource location for the entity enclosed in the message 
        /// when that entity is accessible from a location separate from the requested resource's URI.
        /// </summary>
        public string ContentLocation { get; private set; }

        // TODO :This should be files

        /// <summary>
        /// Http content description.
        /// </summary>
        public string ContentDisposition { get; private set; }

        /// <summary>
        /// Represents the date and time at which the message was originated
        /// </summary>
        public DateTime Date { get; private set; }

        /// <summary>
        /// provides the current value of the entity tag for the requested variant. 
        /// </summary>
        /// <example>ETag: "xyzzy"; ETag: W/"xyzzy"</example>
        public string ETag { get; private set; }

        /// <summary>
        /// Gives the date/time after which the response is considered stale.
        /// </summary>
        /// <example>Expires: Thu, 01 Dec 1994 16:00:00 GMT</example>
        public DateTime Expires { get; private set; }

        /// <summary>
        /// Indicates the date and time at which the origin server believes the variant was last modified.
        /// </summary>
        /// <example>Last-Modified: Tue, 15 Nov 1994 12:45:26 GMT</example>
        public DateTime LastModified { get; private set; }

        /// <summary>
        /// Used to redirect the recipient to a location other than the
        /// Request-URI for completion of the request or identification of a new resource.
        /// </summary>
        /// <example>Location: http://www.w3.org/pub/WWW/People.html </example>
        public string Location { get; private set; }

        /// <summary>
        /// Used to include implementation- specific directives that
        /// might apply to any recipient along the request/response chain.
        /// </summary>
        /// <example> 
        /// pragma-directive  = "no-cache" | extension-pragma
        /// extension-pragma  = token [ "=" ( token | quoted-string ) ]
        /// </example>
        public CacheControl Pragma { get; private set; }

        /// <summary>
        /// Contains information about the software used by the origin server to handle the request. 
        /// </summary>
        /// <example>
        /// Server: CERN/3.0 libwww/2.17
        /// </example>
        public string Server { get; private set; }

        /// <summary>
        /// Response headers in raw format.
        /// </summary>
        public WebHeaderCollection RawHeaders { get; private set; }

        /// <summary>
        /// Response stream.
        /// </summary>
        public Stream ResponseStream
        {
            get
            {
                return this.response.GetResponseStream();
            }
        }

        /// <summary>
        /// Response body decoded as dynamic object.
        /// </summary>
        public dynamic DynamicBody
        {
            get
            {
                return this.decoder.DecodeToDynamic(this.RawText, this.ContentType);
            }
        }

        /// <summary>
        /// Text in raw format
        /// </summary>
        public string RawText { get; private set; }

        /// <summary>
        /// Response body decoded as object.
        /// </summary>
        /// <typeparam name="T">Type of object to decodes as.</typeparam>
        /// <param name="overrideContentType">Override the type of object to decode as.</param>
        /// <returns>The decoded object.</returns>
        public T StaticBody<T>(string overrideContentType = null)
        {
            if (overrideContentType != null)
            {
                return this.decoder.DecodeToStatic<T>(this.RawText, overrideContentType);
            }

            return this.decoder.DecodeToStatic<T>(this.RawText, this.ContentType);
        }

        /// <summary>
        /// Method for getting a http response
        /// </summary>
        /// <param name="request">Http request to be sent</param>
        /// <param name="filename">Name of the file to get</param>
        /// <param name="streamResponse">Is the response a stream</param>
        public void GetResponse(WebRequest request, string filename, bool streamResponse)
        {
            try
            {
                this.response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException webException)
            {
                if (webException.Response == null)
                {
                    throw;
                }

                this.response = (HttpWebResponse)webException.Response;
            }

            this.GetHeaders();

            if (streamResponse)
            {
                return;
            }

            using (var stream = this.response.GetResponseStream())
            {
                if (stream == null)
                {
                    return;
                }

                if (!string.IsNullOrEmpty(filename))
                {
                    using (var filestream = new FileStream(filename, FileMode.CreateNew))
                    {
                        int count;
                        var buffer = new byte[8192];

                        while ((count = stream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            filestream.Write(buffer, 0, count);
                        }
                    }
                }
                else
                {
                    var encoding = string.IsNullOrEmpty(this.CharacterSet)
                                       ? Encoding.UTF8
                                       : Encoding.GetEncoding(this.CharacterSet);
                    using (var reader = new StreamReader(stream, encoding))
                    {
                        this.RawText = reader.ReadToEnd();
                    }
                }
            }
        }

        /// <summary>
        /// Method for setting up the response headers
        /// </summary>
        private void GetHeaders()
        {
            this.CharacterSet = this.response.CharacterSet;
            this.ContentType = this.response.ContentType;
            this.StatusCode = this.response.StatusCode;
            this.StatusDescription = this.response.StatusDescription;
            this.Cookies = this.response.Cookies;
            this.ContentEncoding = this.response.ContentEncoding;
            this.ContentLength = this.response.ContentLength;
            this.Date = DateTime.Now;
            this.LastModified = this.response.LastModified;
            this.Server = this.response.Server;

            if (!string.IsNullOrEmpty(this.GetHeader("Age")))
            {
                this.Age = Convert.ToInt32(this.GetHeader("Age"));
            }

            this.ContentLanguage = this.GetHeader("Content-Language");
            this.ContentLocation = this.GetHeader("Content-Location");
            this.ContentDisposition = this.GetHeader("Content-Disposition");
            this.ETag = this.GetHeader("ETag");
            this.Location = this.GetHeader("Location");

            if (!string.IsNullOrEmpty(this.GetHeader("Expires")))
            {
                DateTime expires;
                if (DateTime.TryParse(this.GetHeader("Expires"), out expires))
                {
                    this.Expires = expires;
                }
            }

            // TODO: Finish this.
            // this.Allow = ...
            // this.CacheControl = ...
            // this.Pragma = ...
            this.RawHeaders = this.response.Headers;
        }

        /// <summary>
        /// Method for getting a response header
        /// </summary>
        /// <param name="header">Name of the header to get</param>
        /// <returns>The value of the header</returns>
        private string GetHeader(string header)
        {
            var headerValue = this.response.GetResponseHeader(header);

            return headerValue.Replace("\"", string.Empty);
        }
    }
}