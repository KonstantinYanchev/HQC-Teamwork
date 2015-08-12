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

using Result = EasyHttp.Specs.Helpers.ResultResponse;

namespace EasyHttp.Specs.Specs
{
    using System;
    using System.Net;

    using EasyHttp.Http;
    using EasyHttp.Specs.Helpers;

    [Subject("HttpClient")]
    public class when_making_any_type_of_request_to_invalid_host
    {
        private static HttpClient httpClient;

        private static Exception exception;

        private Establish context = () => { httpClient = new HttpClient(); };

        private Because of = () => { exception = Catch.Exception(() => httpClient.Get("http://somethinginvalid")); };

        private It should_throw_web_exception = () => exception.ShouldBeOfType<WebException>();
    }

    [Subject("HttpClient")]
    public class when_making_a_DELETE_request_with_a_valid_uri
    {
        // TODO: Implement me
        private static HttpClient httpClient;

        private static dynamic response;

        private static string rev;

        private static Guid guid;
    }

    [Subject("HttpClient")]
    public class when_making_a_GET_request_with_valid_uri
    {
        private static HttpClient httpClient;

        private static HttpResponse httpResponse;

        private Establish context = () => { httpClient = new HttpClient(); };

        private Because of = () => { httpResponse = httpClient.Get("http://localhost:16000"); };

        private It should_return_body_with_rawtext = () => httpResponse.RawText.ShouldNotBeEmpty();
    }

    [Subject("HttpClient")]
    public class when_making_a_GET_request_with_valid_uri_and__and_valid_parameters
    {
        private static HttpClient httpClient;

        private static dynamic response;

        private Establish context = () =>
            {
                httpClient = new HttpClient();
                httpClient.Request.Accept = HttpContentTypes.ApplicationJson;
            };

        private Because of = () => { response = httpClient.Get("http://localhost:16000/hello", new { Name = "true" }); };

        private It should_return_dynamic_body_with_json_object = () =>
            {
                dynamic body = response.DynamicBody;

                string couchdb = body.Result;

                couchdb.ShouldEqual("Hello, true");
            };
    }

    [Subject("HttpClient")]
    public class when_making_a_GET_request_with_valid_uri_and__and_valid_parameters_using_segments
    {
        private static HttpClient httpClient;

        private static dynamic response;

        private Establish context = () =>
            {
                httpClient = new HttpClient();
                httpClient.Request.Accept = HttpContentTypes.ApplicationJson;
                httpClient.Request.ParametersAsSegments = true;
            };

        private Because of = () => { response = httpClient.Get("http://localhost:16000/hello", new { Name = "true" }); };

        private It should_return_dynamic_body_with_json_object = () =>
            {
                dynamic body = response.DynamicBody;

                string couchdb = body.Result;

                couchdb.ShouldEqual("Hello, true");
            };
    }

    [Subject("HttpClient")]
    public class when_making_a_GET_request_with_valid_uri_and_content_type_set_to_application_json
    {
        private static HttpClient httpClient;

        private static HttpResponse response;

        private Establish context = () =>
            {
                httpClient = new HttpClient();
                httpClient.Request.Accept = HttpContentTypes.ApplicationJson;
            };

        private Because of = () => { response = httpClient.Get("http://localhost:16000/hello"); };

        private It should_return_dynamic_body_with_json_object = () =>
            {
                dynamic body = response.DynamicBody;

                string result = body.Result;

                result.ShouldEqual("Hello, ");
            };

        private It should_return_static_body_with_json_object = () =>
            {
                var couchInformation = response.StaticBody<ResultResponse>();

                couchInformation.Result.ShouldEqual("Hello, ");
            };
    }

    [Subject("HttpClient")]
    public class when_making_a_HEAD_request_with_valid_uri
    {
        private static HttpClient httpClient;

        private static HttpResponse response;

        private Establish context = () => { httpClient = new HttpClient(); };

        private Because of = () => { response = httpClient.Head("http://localhost:16000"); };

        private It should_return_OK_response = () => response.StatusDescription.ShouldEqual("OK");
    }

    [Subject("HttpClient")]
    public class when_making_a_POST_request_with_valid_uri_and_valid_data_and_content_type_set_to_application_json
    {
        private static HttpClient httpClient;

        private static dynamic response;

        private Establish context = () =>
            {
                httpClient = new HttpClient();
                httpClient.Request.Accept = HttpContentTypes.ApplicationJson;
            };

        private Because of =
            () =>
                {
                    response = httpClient.Post(
                        "http://localhost:16000/hello", 
                        new Customer { Name = "Hadi" }, 
                        HttpContentTypes.ApplicationJson);
                };

        private It should_succeed = () =>
            {
                string id = response.DynamicBody.Result;

                id.ShouldNotBeEmpty();
            };
    }

    [Subject("HttpClient")]
    public class
        when_making_a_POST_request_with_valid_uri_and_valid_data_and_content_type_set_to_application_json_and_parameters_as_segments
    {
        private static HttpClient httpClient;

        private static dynamic response;

        private Establish context = () =>
            {
                httpClient = new HttpClient();
                httpClient.Request.Accept = HttpContentTypes.ApplicationJson;
                httpClient.Request.ParametersAsSegments = true;
            };

        private Because of =
            () =>
                {
                    response = httpClient.Post(
                        "http://localhost:16000/hello", 
                        new Customer { Name = "Hadi" }, 
                        HttpContentTypes.ApplicationJson);
                };

        private It should_succeed = () =>
            {
                string id = response.DynamicBody.Result;

                id.ShouldNotBeEmpty();
            };
    }

    [Subject("HttpClient")]
    public class when_making_a_PUT_request_with_valid_uri_and_valid_data_and_content_type_set_to_application_json
    {
        private static HttpClient httpClient;

        private static dynamic response;

        private Establish context = () =>
            {
                httpClient = new HttpClient();
                httpClient.Request.Accept = HttpContentTypes.ApplicationJson;
            };

        private Because of =
            () =>
                {
                    response = httpClient.Put(
                        string.Format("{0}/{1}", "http://localhost:16000", "hello"), 
                        new Customer { Name = "Put" }, 
                        HttpContentTypes.ApplicationJson);
                };

        private It should_succeed = () =>
            {
                string result = response.DynamicBody.Result;

                result.ShouldNotBeEmpty();
            };
    }

    [Subject("HttpClient")]
    public class when_making_requests_and_persisting_cookies
    {
        private static HttpClient httpClient;

        private static dynamic response;

        private Establish context = () =>
            {
                httpClient = new HttpClient();
                httpClient.Request.PersistCookies = true;
                httpClient.Request.Accept = HttpContentTypes.ApplicationJson;
            };

        private Because of = () =>
            {
                httpClient.Put(
                    "http://localhost:16000/cookie", 
                    new CookieInfo { Name = "test", Value = "test cookie" }, 
                    HttpContentTypes.ApplicationJson);
                response = httpClient.Get("http://localhost:16000/cookie/test");
            };

        private It should_send_returned_cookies = () =>
            {
                string cookieValue = response.DynamicBody.Value;

                cookieValue.ShouldEqual("test cookie");
            };
    }

    [Subject("HttpClient")]
    public class when_making_requests_and_not_persisting_cookies
    {
        private static HttpClient httpClient;

        private static dynamic response;

        private Establish context = () =>
            {
                httpClient = new HttpClient();
                httpClient.Request.PersistCookies = false;
                httpClient.Request.Accept = HttpContentTypes.ApplicationJson;
            };

        private Because of = () =>
            {
                httpClient.Put(
                    "http://localhost:16000/cookie", 
                    new CookieInfo { Name = "test", Value = "test cookie" }, 
                    HttpContentTypes.ApplicationJson);
                response = httpClient.Get("http://localhost:16000/cookie/test");
            };

        private It should_not_send_returned_cookies = () =>
            {
                HttpStatusCode statusCode = response.StatusCode;

                statusCode.ShouldEqual(HttpStatusCode.NotFound);
            };
    }
}