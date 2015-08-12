namespace EasyHttp.Infrastructure
{
    using EasyHttp.Http;

    public class FileData
    {
        public FileData()
        {
            this.ContentTransferEncoding = HttpContentTransferEncoding.Binary;
        }

        public string FieldName { get; set; }

        public string Filename { get; set; }

        public string ContentType { get; set; }

        public string ContentTransferEncoding { get; set; }
    }
}