namespace EasyHttp.Codecs.JsonFXExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using JsonFx.Serialization;
    using JsonFx.Serialization.Providers;

    /// <summary>
    /// Data reader provider that usings regular expressions.
    /// </summary>
    public class RegExBasedDataReaderProvider : IDataReaderProvider
    {
        private readonly IDictionary<string, IDataReader> _readersByMime =
            new Dictionary<string, IDataReader>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Data reader provider that usings regular expressions.
        /// </summary>
        /// <param name="dataReaders">Collection of DataReaders that will be added to the class by Mime Type.</param>
        public RegExBasedDataReaderProvider(IEnumerable<IDataReader> dataReaders)
        {
            if (dataReaders != null)
            {
                foreach (var reader in dataReaders)
                {
                    foreach (var contentType in reader.ContentType)
                    {
                        if (string.IsNullOrEmpty(contentType) || this._readersByMime.ContainsKey(contentType))
                        {
                            continue;
                        }

                        this._readersByMime[contentType] = reader;
                    }
                }
            }
        }

        /// <summary>
        /// Get a data reader by the content type provider in the content type header.
        /// </summary>
        /// <param name="contentTypeHeader">String from which to get the content type.</param>
        /// <returns>DataReader that corresponds to the required content type if such exist. Otherwise returns null.</returns>
        public IDataReader Find(string contentTypeHeader)
        {
            var type = DataProviderUtility.ParseMediaType(contentTypeHeader);

            var readers = this._readersByMime.Where(reader => Regex.Match(type, reader.Key, RegexOptions.Singleline).Success);

            return readers.Any() ? readers.First().Value : null;
        }
    }
}