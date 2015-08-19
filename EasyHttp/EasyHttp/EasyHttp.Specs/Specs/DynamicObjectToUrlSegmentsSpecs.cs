namespace EasyHttp.Specs.Specs
{
    using System.Dynamic;

    using Machine.Specifications;

    using EasyHttp.Http;
    using EasyHttp.Infrastructure;

    [Subject(typeof(HttpClient))]
    public class when_making_url_segments_with_one_parameter_using_expando_object
    {
        private static ObjectToUrlSegments objectToUrlSegments;

        private static dynamic parameters;

        private static string url;

        private Establish context = () =>
            {
                objectToUrlSegments = new ObjectToUrlSegments();
                parameters = new ExpandoObject();
                parameters.Name = "test";
            };

        private Because of = () => url = objectToUrlSegments.ParametersToUrl(parameters);

        private It should_have_the_correct_url_parameters = () => url.ShouldEqual("/test");
    }

    [Subject(typeof(HttpClient))]
    public class when_making_url_segments_with_two_parameters_using_expando_object
    {
        private static ObjectToUrlSegments objectToUrlSegments;

        private static dynamic parameters;

        private static string url;

        private Establish context = () =>
            {
                objectToUrlSegments = new ObjectToUrlSegments();
                parameters = new ExpandoObject();
                parameters.Name = "test";
                parameters.Id = 1;
            };

        private Because of = () => url = objectToUrlSegments.ParametersToUrl(parameters);

        private It should_have_the_correct_url_parameters = () => url.ShouldEqual("/test/1");
    }

    [Subject(typeof(HttpClient))]
    public class when_making_url_segments_with_one_parameter_using_anonymous_object
    {
        private static ObjectToUrlSegments _objectToUrlSegments;

        private static string url;

        private Establish context = () => { _objectToUrlSegments = new ObjectToUrlSegments(); };

        private Because of = () => url = _objectToUrlSegments.ParametersToUrl(new { Name = "test" });

        private It should_have_the_correct_url_segments = () => url.ShouldEqual("/test");
    }

    [Subject(typeof(HttpClient))]
    public class when_making_url_segments_with_two_parameters_using_anonymous_object
    {
        private static ObjectToUrlSegments _objectToUrlSegments;

        private static string url;

        private Establish context = () => { _objectToUrlSegments = new ObjectToUrlSegments(); };

        private Because of = () => url = _objectToUrlSegments.ParametersToUrl(new { Name = "test", Id = 1 });

        private It should_have_the_correct_url_segments = () => url.ShouldEqual("/test/1");
    }

    [Subject(typeof(HttpClient))]
    public class when_making_url_segments_it_should_encode_value
    {
        private static ObjectToUrlSegments _objectToUrlSegments;

        private static string url;

        private Establish context = () => { _objectToUrlSegments = new ObjectToUrlSegments(); };

        private Because of = () => url = _objectToUrlSegments.ParametersToUrl(new { Name = "test<>&;" });

        private It should_have_the_correct_url_segments = () => url.ShouldEqual("/test%3c%3e%26%3b");
    }

    [Subject(typeof(HttpClient))]
    public class when_making_url_segments_it_should_be_empty_when_passing_null
    {
        private static ObjectToUrlSegments _objectToUrlSegments;

        private static string url;

        private Establish context = () => { _objectToUrlSegments = new ObjectToUrlSegments(); };

        private Because of = () => url = _objectToUrlSegments.ParametersToUrl(null);

        private It should_have_the_correct_url_segments = () => url.ShouldBeEmpty();
    }

    [Subject(typeof(HttpClient))]
    public class when_making_url_segments_with_one_parameter_using_static_object
    {
        private static ObjectToUrlSegments _objectToUrlSegments;

        private static string url;

        private static StaticObjectWithName parameter;

        private Establish context = () =>
            {
                _objectToUrlSegments = new ObjectToUrlSegments();
                parameter = new StaticObjectWithName { Name = "test" };
            };

        private Because of = () => url = _objectToUrlSegments.ParametersToUrl(parameter);

        private It should_have_the_correct_url_segments = () => url.ShouldEqual("/test");
    }

    [Subject(typeof(HttpClient))]
    public class when_making_url_segments_with_two_parameters_using_static_object
    {
        private static ObjectToUrlSegments _objectToUrlSegments;

        private static string url;

        private static StaticObjectWithNameAndId parameter;

        private Establish context = () =>
            {
                _objectToUrlSegments = new ObjectToUrlSegments();
                parameter = new StaticObjectWithNameAndId { Name = "test", Id = 1 };
            };

        private Because of = () => url = _objectToUrlSegments.ParametersToUrl(parameter);

        private It should_have_the_correct_url_segments = () => url.ShouldEqual("/test/1");
    }

    [Subject(typeof(HttpClient))]
    public class when_making_url_segments_with_two_parameters_using_static_object_in_different_order
    {
        private static ObjectToUrlSegments _objectToUrlSegments;

        private static string url;

        private static StaticObjectWithNameAndIdInDifferentOrder parameter;

        private Establish context = () =>
            {
                _objectToUrlSegments = new ObjectToUrlSegments();
                parameter = new StaticObjectWithNameAndIdInDifferentOrder { Name = "test", Id = 1 };
            };

        private Because of = () => url = _objectToUrlSegments.ParametersToUrl(parameter);

        private It should_have_the_correct_url_segments = () => url.ShouldEqual("/1/test");
    }

    public class StaticObjectWithNameAndIdInDifferentOrder
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}