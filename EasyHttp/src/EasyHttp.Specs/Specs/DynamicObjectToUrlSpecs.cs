namespace EasyHttp.Specs.Specs
{
    using System.Dynamic;

    using EasyHttp.Http;
    using EasyHttp.Infrastructure;

    [Subject(typeof(HttpClient))]
    public class when_making_url_parameters_with_one_parameter_using_expando_object
    {
        private static ObjectToUrlParameters objectToUrlParameters;

        private static dynamic parameters;

        private static string url;

        private Establish context = () =>
            {
                objectToUrlParameters = new ObjectToUrlParameters();
                parameters = new ExpandoObject();
                parameters.Name = "test";
            };

        private Because of = () => url = objectToUrlParameters.ParametersToUrl(parameters);

        private It should_have_the_correct_url_parameters = () => url.ShouldEqual("?Name=test");
    }

    [Subject(typeof(HttpClient))]
    public class when_making_url_parameters_with_two_parameters_using_expando_object
    {
        private static ObjectToUrlParameters objectToUrlParameters;

        private static dynamic parameters;

        private static string url;

        private Establish context = () =>
            {
                objectToUrlParameters = new ObjectToUrlParameters();
                parameters = new ExpandoObject();
                parameters.Name = "test";
                parameters.Id = 1;
            };

        private Because of = () => url = objectToUrlParameters.ParametersToUrl(parameters);

        private It should_have_the_correct_url_parameters = () => url.ShouldEqual("?Name=test&Id=1");
    }

    [Subject(typeof(HttpClient))]
    public class when_making_url_parameters_with_one_parameter_using_anonymous_object
    {
        private static ObjectToUrlParameters objectToUrlParameters;

        private static string url;

        private Establish context = () => { objectToUrlParameters = new ObjectToUrlParameters(); };

        private Because of = () => url = objectToUrlParameters.ParametersToUrl(new { Name = "test" });

        private It should_have_the_correct_url_parameters = () => url.ShouldEqual("?Name=test");
    }

    [Subject(typeof(HttpClient))]
    public class when_making_url_parameters_with_two_parameters_using_anonymous_object
    {
        private static ObjectToUrlParameters objectToUrlParameters;

        private static string url;

        private Establish context = () => { objectToUrlParameters = new ObjectToUrlParameters(); };

        private Because of = () => url = objectToUrlParameters.ParametersToUrl(new { Name = "test", Id = 1 });

        private It should_have_the_correct_url_parameters = () => url.ShouldEqual("?Name=test&Id=1");
    }

    [Subject(typeof(HttpClient))]
    public class when_making_url_parameters_it_should_encode_value
    {
        private static ObjectToUrlParameters objectToUrlParameters;

        private static string url;

        private Establish context = () => { objectToUrlParameters = new ObjectToUrlParameters(); };

        private Because of = () => url = objectToUrlParameters.ParametersToUrl(new { Name = "test<>&;" });

        private It should_have_the_correct_url_parameters = () => url.ShouldEqual("?Name=test%3c%3e%26%3b");
    }

    [Subject(typeof(HttpClient))]
    public class when_making_url_parameters_it_should_be_empty_when_passing_null
    {
        private static ObjectToUrlParameters objectToUrlParameters;

        private static string url;

        private Establish context = () => { objectToUrlParameters = new ObjectToUrlParameters(); };

        private Because of = () => url = objectToUrlParameters.ParametersToUrl(null);

        private It should_have_the_correct_url_parameters = () => url.ShouldBeEmpty();
    }

    [Subject(typeof(HttpClient))]
    public class when_making_url_parameters_with_one_parameter_using_static_object
    {
        private static ObjectToUrlParameters objectToUrlParameters;

        private static string url;

        private static StaticObjectWithName parameter;

        private Establish context = () =>
            {
                objectToUrlParameters = new ObjectToUrlParameters();
                parameter = new StaticObjectWithName { Name = "test" };
            };

        private Because of = () => url = objectToUrlParameters.ParametersToUrl(parameter);

        private It should_have_the_correct_url_parameters = () => url.ShouldEqual("?Name=test");
    }

    [Subject(typeof(HttpClient))]
    public class when_making_url_parameters_with_two_parameters_using_static_object
    {
        private static ObjectToUrlParameters objectToUrlParameters;

        private static string url;

        private static StaticObjectWithNameAndId parameter;

        private Establish context = () =>
            {
                objectToUrlParameters = new ObjectToUrlParameters();
                parameter = new StaticObjectWithNameAndId { Name = "test", Id = 1 };
            };

        private Because of = () => url = objectToUrlParameters.ParametersToUrl(parameter);

        private It should_have_the_correct_url_parameters = () => url.ShouldEqual("?Name=test&Id=1");
    }

    public class StaticObjectWithName
    {
        public string Name { get; set; }
    }

    public class StaticObjectWithNameAndId
    {
        public string Name { get; set; }

        public int Id { get; set; }
    }
}