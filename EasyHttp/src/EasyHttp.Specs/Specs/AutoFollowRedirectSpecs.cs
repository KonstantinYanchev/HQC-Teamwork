namespace EasyHttp.Specs.Specs
{
    using System.Net;

    using EasyHttp.Http;

    public class AutoFollowRedirectSpecs
    {
        [Subject("HttpClient")]
        public class when_making_a_GET_request_with_AutoRedirect_on
        {
            private static HttpClient httpClient;

            private Establish context = () => { httpClient = new HttpClient(); };

            private Because of = () => httpClient.Get("http://localhost:16000/redirector");

            private It should_redirect = () => httpClient.Response.Location.ShouldBeEmpty();

            private It should_return_status_code_of_OK =
                () => httpClient.Response.StatusCode.ShouldEqual(HttpStatusCode.OK);
        }

        [Subject("HttpClient")]
        public class when_making_a_GET_request_with_AutoRedirect_off
        {
            private static HttpClient httpClient;

            private Establish context = () => { httpClient = new HttpClient(); };

            private Because of = () =>
                {
                    httpClient.Request.AllowAutoRedirect = false;
                    httpClient.Get("http://localhost:16000/redirector");
                };

            private It should_redirect = () => httpClient.Response.Location.ShouldEndWith("redirected");

            private It should_return_status_code_of_Redirect =
                () => httpClient.Response.StatusCode.ShouldEqual(HttpStatusCode.Redirect);
        }
    }
}