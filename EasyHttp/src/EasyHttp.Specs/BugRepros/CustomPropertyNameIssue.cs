namespace EasyHttp.Specs.BugRepros
{
    using System.Collections.Generic;
    using System.Text;

    using EasyHttp.Codecs;
    using EasyHttp.Codecs.JsonFXExtensions;
    using EasyHttp.Http;

    [Subject("Custom Decoding")]
    public class when_decoding_an_object_with_custom_naming_of_property
    {
        private static IDecoder decoder;

        private static CustomNaming obj;

        private Establish context = () =>
            {
                IEnumerable<IDataReader> readers = new List<IDataReader>
                                                       {
                                                           new JsonReader(
                                                               new DataReaderSettings(
                                                               CombinedResolverStrategy()), 
                                                               HttpContentTypes.ApplicationJson)
                                                       };

                decoder = new DefaultDecoder(new RegExBasedDataReaderProvider(readers));
            };

        private Because of =
            () => { obj = decoder.DecodeToStatic<CustomNaming>("{\"abc\":\"def\"}", "application/json"); };

        private It should_decode_taking_into_account_custom_property_name =
            () => { obj.PropertyName.ShouldEqual("def"); };

        private static CombinedResolverStrategy CombinedResolverStrategy()
        {
            return new CombinedResolverStrategy(
                new JsonResolverStrategy(), 
                new DataContractResolverStrategy(), 
                new XmlResolverStrategy(), 
                new ConventionResolverStrategy(ConventionResolverStrategy.WordCasing.PascalCase), 
                new ConventionResolverStrategy(ConventionResolverStrategy.WordCasing.CamelCase), 
                new ConventionResolverStrategy(ConventionResolverStrategy.WordCasing.Lowercase, "-"), 
                new ConventionResolverStrategy(ConventionResolverStrategy.WordCasing.Uppercase, "_"));
        }
    }

    [Subject("Custom Encoding")]
    public class when_encoding_an_object_with_custom_naming_of_property
    {
        private static IEncoder encoder;

        private static byte[] encoded;

        private Establish context = () =>
            {
                IEnumerable<IDataWriter> writers = new List<IDataWriter>
                                                       {
                                                           new JsonWriter(
                                                               new DataWriterSettings(
                                                               CombinedResolverStrategy()), 
                                                               HttpContentTypes.ApplicationJson)
                                                       };

                encoder = new DefaultEncoder(new RegExBasedDataWriterProvider(writers));
            };

        private Because of = () =>
            {
                var customObject = new CustomNamedObject { UpperPropertyName = "someValue" };

                encoded = encoder.Encode(customObject, HttpContentTypes.ApplicationJson);
            };

        private It should_decode_taking_into_account_custom_property_name = () =>
            {
                var str = Encoding.UTF8.GetString(encoded);
                str.ShouldContain("upperPropertyName");
            };

        private static CombinedResolverStrategy CombinedResolverStrategy()
        {
            return new CombinedResolverStrategy(
                new JsonResolverStrategy(), 
                new DataContractResolverStrategy(), 
                new XmlResolverStrategy(), 
                new ConventionResolverStrategy(ConventionResolverStrategy.WordCasing.PascalCase), 
                new ConventionResolverStrategy(ConventionResolverStrategy.WordCasing.CamelCase), 
                new ConventionResolverStrategy(ConventionResolverStrategy.WordCasing.Lowercase, "-"), 
                new ConventionResolverStrategy(ConventionResolverStrategy.WordCasing.Uppercase, "_"));
        }
    }

    public class CustomNamedObject
    {
        [JsonName("upperPropertyName")]
        public string UpperPropertyName { get; set; }
    }

    public class CustomNaming
    {
        [JsonName("abc")]
        public string PropertyName { get; set; }
    }
}