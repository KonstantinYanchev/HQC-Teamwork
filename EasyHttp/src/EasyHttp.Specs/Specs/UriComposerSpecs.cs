namespace EasyHttp.Specs.Specs
{
    using EasyHttp.Infrastructure;

    public class When_baseuri_is_null_and_query_is_null
    {
        private static UriComposer uriComposer;

        private static string url;

        private static string uri;

        private Establish context = () =>
            {
                uriComposer = new UriComposer();
                uri = "uri";
            };

        private Because of = () => url = uriComposer.Compose(null, uri, null, false);

        private It should_return_the_uri = () => url.ShouldEqual("uri");
    }

    public class When_baseuri_is_empty_and_query_is_null
    {
        private static UriComposer uriComposer;

        private static string url;

        private static string uri;

        private static string baseuri;

        private Establish context = () =>
            {
                uriComposer = new UriComposer();
                baseuri = string.Empty;
                uri = "uri";
            };

        private Because of = () => url = uriComposer.Compose(baseuri, uri, null, false);

        private It should_return_the_uri = () => url.ShouldEqual("uri");
    }

    public class When_baseuri_is_filled_and_does_not_end_with_a_forwardslash_and_query_is_null
    {
        private static UriComposer uriComposer;

        private static string url;

        private static string uri;

        private static string baseuri;

        private Establish context = () =>
            {
                uriComposer = new UriComposer();
                baseuri = "baseuri";
                uri = "uri";
            };

        private Because of = () => url = uriComposer.Compose(baseuri, uri, null, false);

        private It should_return_the_baseuri_plus_uri = () => url.ShouldEqual("baseuri/uri");
    }

    public class When_baseuri_is_filled_and_ends_with_a_forwardslash_and_query_is_null
    {
        private static UriComposer uriComposer;

        private static string url;

        private static string uri;

        private static string baseuri;

        private Establish context = () =>
            {
                uriComposer = new UriComposer();
                baseuri = "baseuri/";
                uri = "uri";
            };

        private Because of = () => url = uriComposer.Compose(baseuri, uri, null, false);

        private It should_return_the_baseuri_plus_uri = () => url.ShouldEqual("baseuri/uri");
    }

    public class
        When_baseuri_is_filled_and_ends_with_a_forwardslash_and_uri_starartswith_a_forwardslash_and_query_is_null
    {
        private static UriComposer uriComposer;

        private static string url;

        private static string uri;

        private static string baseuri;

        private Establish context = () =>
            {
                uriComposer = new UriComposer();
                baseuri = "baseuri/";
                uri = "/uri";
            };

        private Because of = () => url = uriComposer.Compose(baseuri, uri, null, false);

        private It should_return_the_baseuri_plus_uri = () => url.ShouldEqual("baseuri/uri");
    }

    public class
        When_baseuri_is_filled_and_does_not_end_with_a_forwardslash_and_uri_starartswith_a_forwardslash_and_query_is_null
    {
        private static UriComposer uriComposer;

        private static string url;

        private static string uri;

        private static string baseuri;

        private Establish context = () =>
            {
                uriComposer = new UriComposer();
                baseuri = "baseuri";
                uri = "/uri";
            };

        private Because of = () => url = uriComposer.Compose(baseuri, uri, null, false);

        private It should_return_the_baseuri_plus_uri = () => url.ShouldEqual("baseuri/uri");
    }

    public class When_baseuri_and_url_are_filled_and_query_is_not_null
    {
        private static UriComposer uriComposer;

        private static string url;

        private static string uri;

        private static string baseuri;

        private static object query;

        private Establish context = () =>
            {
                uriComposer = new UriComposer();
                baseuri = "baseuri";
                uri = "/uri";
                query = new { Name = "test" };
            };

        private Because of = () => url = uriComposer.Compose(baseuri, uri, query, false);

        private It should_return_the_baseuri_plus_uri = () => url.ShouldEqual("baseuri/uri?Name=test");
    }

    public class When_baseuri_and_url_are_filled_and_query_is_not_null_and_ParametersAsSegments_is_true
    {
        private static UriComposer uriComposer;

        private static string url;

        private static string uri;

        private static string baseuri;

        private static object query;

        private Establish context = () =>
            {
                uriComposer = new UriComposer();
                baseuri = "baseuri";
                uri = "/uri";
                query = new { Name = "test" };
            };

        private Because of = () => url = uriComposer.Compose(baseuri, uri, query, true);

        private It should_return_the_baseuri_plus_uri = () => url.ShouldEqual("baseuri/uri/test");
    }
}