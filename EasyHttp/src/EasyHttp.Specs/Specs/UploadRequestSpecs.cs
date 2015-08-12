namespace EasyHttp.Specs.Specs
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;

    using EasyHttp.Http;
    using EasyHttp.Infrastructure;

    [Subject(typeof(HttpClient))]
    public class when_sending_binary_data_as_put
    {
        private static HttpClient httpClient;

        private static Guid guid;

        private static HttpResponse response;

        private static string rev;

        private Establish context = () => { httpClient = new HttpClient(); };

        private Because of = () =>
            {
                var imageFile = Path.Combine("Helpers", "test.jpg");

                httpClient.PutFile(
                    string.Format("{0}/fileupload/test.jpg", "http://localhost:16000"), 
                    imageFile, 
                    "image/jpeg");
            };

        private It should_upload_it_succesfully =
            () => { httpClient.Response.StatusCode.ShouldEqual(HttpStatusCode.Created); };
    }

    [Subject(typeof(HttpClient))]
    public class when_sending_binary_data_as_multipart_post
    {
        private static HttpClient httpClient;

        private static Guid guid;

        private static HttpResponse response;

        private static string rev;

        private Establish context = () => { httpClient = new HttpClient(); };

        private Because of = () =>
            {
                var imageFile = Path.Combine("Helpers", "test.jpg");

                IDictionary<string, object> data = new Dictionary<string, object>();

                data.Add("email", "hadi@hadi.com");
                data.Add("name", "hadi");

                IList<FileData> files = new List<FileData>();

                files.Add(new FileData { FieldName = "image1", ContentType = "image/jpeg", Filename = imageFile });
                files.Add(new FileData { FieldName = "image2", ContentType = "image/jpeg", Filename = imageFile });
                httpClient.Post(string.Format("{0}/fileupload", "http://localhost:16000"), data, files);
            };

        private It should_upload_it_succesfully =
            () => { httpClient.Response.StatusCode.ShouldEqual(HttpStatusCode.OK); };
    }
}