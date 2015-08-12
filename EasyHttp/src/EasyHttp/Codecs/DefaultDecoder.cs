namespace EasyHttp.Codecs
{
    using System;

    public class DefaultDecoder : IDecoder
    {
        private readonly IDataReaderProvider _dataReaderProvider;

        public DefaultDecoder(IDataReaderProvider dataReaderProvider)
        {
            this._dataReaderProvider = dataReaderProvider;
        }

        public T DecodeToStatic<T>(string input, string contentType)
        {
            var parsedText = NormalizeInputRemovingAmpersands(input);

            var deserializer = this.ObtainDeserializer(contentType);

            return deserializer.Read<T>(parsedText);
        }

        public dynamic DecodeToDynamic(string input, string contentType)
        {
            var parsedText = NormalizeInputRemovingAmpersands(input);

            var deserializer = this.ObtainDeserializer(contentType);

            return deserializer.Read(parsedText);
        }

        private IDataReader ObtainDeserializer(string contentType)
        {
            var deserializer = this._dataReaderProvider.Find(contentType);

            if (deserializer == null)
            {
                throw new SerializationException("The encoding requested does not have a corresponding decoder");
            }

            return deserializer;
        }

        private static string NormalizeInputRemovingAmpersands(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException("input");
            }

            // this is a hack 
            var parsedText = input.Replace("\"@", "\"");
            return parsedText;
        }
    }
}