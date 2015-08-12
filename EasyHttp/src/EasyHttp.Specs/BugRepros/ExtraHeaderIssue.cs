namespace EasyHttp.Specs.BugRepros
{
    using Machine.Specifications;

    using EasyHttp.Http;

    public class when_adding_extra_header_to_request
    {
        private static HttpClient http;

        private Establish context = () => { http = new HttpClient(); };

        private Because of = () => { http.Request.AddExtraHeader("X-Header", "X-Value"); };

        private It should_add_it_to_the_request = () => { http.Request.RawHeaders.ContainsKey("X-Header"); };
    }
}