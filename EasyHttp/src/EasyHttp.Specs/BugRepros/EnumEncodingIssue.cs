namespace EasyHttp.Specs.BugRepros
{
    using System.Collections.Generic;

    using EasyHttp.Codecs;
    using EasyHttp.Codecs.JsonFXExtensions;
    using EasyHttp.Http;

    [Subject("Encoding Enums")]
    public class when_encoding_an_object_that_contains_an_enum
    {
        private static HttpClient client;

        private static DefaultEncoder _encoder;

        private static byte[] result;

        private Establish context = () =>
            {
                IEnumerable<IDataWriter> writers = new List<IDataWriter>
                                                       {
                                                           new JsonWriter(
                                                               new DataWriterSettings(), 
                                                               "application/.*json")
                                                       };

                _encoder = new DefaultEncoder(new RegExBasedDataWriterProvider(writers));
            };

        private Because of = () =>
            {
                var data = new Foo { Baz = Bar.First };

                result = _encoder.Encode(data, "application/vnd.fubar+json");
            };

        private It should_encode_correctly = () => { result.Length.ShouldBeGreaterThan(0); };
    }

    public class Foo
    {
        public Bar Baz { get; set; }
    }

    public enum Bar
    {
        First, 

        Second, 

        Third
    }
}