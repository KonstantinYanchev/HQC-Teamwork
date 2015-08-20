namespace EasyHttp.Specs.Specs
{
    using Machine.Specifications;

    using EasyHttp.Http;

    [Subject(typeof(HttpClient))]
    public class when_making_a_GET_with_stream_response_true
    {
        private static HttpClient httpClient;
    }
}