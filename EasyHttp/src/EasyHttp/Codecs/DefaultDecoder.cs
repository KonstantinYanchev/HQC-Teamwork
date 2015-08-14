namespace EasyHttp.Codecs
{
    using System;

    using JsonFx.Serialization;
    using JsonFx.Serialization.Providers;

    using EasyHttp.Contracts;

    public class DefaultDecoder : IDecoder
    {
        private readonly IDataReaderProvider _dataReaderProvider;

        /// <summary>
        /// Default decoder.
        /// </summary>
        /// <param name="dataReaderProvider">Object which provides lookup capabilities for finding matching IDataReader.</param>
        public DefaultDecoder(IDataReaderProvider dataReaderProvider)
        {
            this._dataReaderProvider = dataReaderProvider;
        }

        /// <summary>
        /// Method for decoding serialized data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input">The serialized data for decoding.</param>
        /// <param name="contentType">Serialization content type.</param>
        /// <returns>Deserialized object.</returns>
        public T DecodeToStatic<T>(string input, string contentType)
        {
            var parsedText = NormalizeInputRemovingAmpersands(input);

            var deserializer = this.ObtainDeserializer(contentType);

            return deserializer.Read<T>(parsedText);
        }

        /// <summary>
        /// Method for decoding serialized data.
        /// </summary>
        /// <param name="input">The serialized data for decoding.</param>
        /// <param name="contentType">Serialization content type.</param>
        /// <returns>Deserialized object as dynamic.</returns>
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