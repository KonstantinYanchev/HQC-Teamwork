namespace EasyHttp.Infrastructure
{
    using EasyHttp.Http;

    /// <summary>
    /// Holds information for a file.
    /// </summary>
    public class FileData
    {
        /// <summary>
        /// Holds information for a file.
        /// </summary>
        public FileData()
        {
            this.ContentTransferEncoding = HttpContentTransferEncoding.Binary;
        }

        public string FieldName { get; set; }

        /// <summary>
        /// Name of the file
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Content type of the file
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Content transfer Encoding.
        /// </summary>
        public string ContentTransferEncoding { get; set; }
    }
}