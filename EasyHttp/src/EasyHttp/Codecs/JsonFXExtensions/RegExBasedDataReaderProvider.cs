namespace EasyHttp.Codecs.JsonFXExtensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using JsonFx.Serialization;
    using JsonFx.Serialization.Providers;

    public class RegExBasedDataReaderProvider : IDataReaderProvider
    {
        private readonly IDictionary<string, IDataReader> _readersByMime =
            new Dictionary<string, IDataReader>(StringComparer.OrdinalIgnoreCase);

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

        public IDataReader Find(string contentTypeHeader)
        {
            var type = DataProviderUtility.ParseMediaType(contentTypeHeader);

            var readers = this._readersByMime.Where(reader => Regex.Match(type, reader.Key, RegexOptions.Singleline).Success);

            return readers.Any() ? readers.First().Value : null;
        }
    }
}