namespace EasyHttp.Specs.Specs
{
    using EasyHttp.Http;

    [Subject("HttpClient Init")]
    public class when_creating_a_new_instance
    {
        private static HttpClient httpClient;

        private Because of = () => { httpClient = new HttpClient(); };

        private It should_return_new_instance_using_default_configuration = () => httpClient.ShouldNotBeNull();
    }

    [Subject("Initializing with base url")]
    public class when_creating_a_new_instance_with_base_url
    {
        private static HttpClient httpClient;

        private Because of = () =>
            {
                httpClient = new HttpClient("http://localhost:16000");

                httpClient.Get("/hello");
            };

        private It should_prefix_all_requests_with_the_base_url =
            () => httpClient.Request.Uri.ShouldEqual("http://localhost:16000/hello");
    }
}