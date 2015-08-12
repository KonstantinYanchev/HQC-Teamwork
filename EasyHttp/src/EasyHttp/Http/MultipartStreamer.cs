namespace EasyHttp.Http
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    using EasyHttp.Infrastructure;

    public class MultiPartStreamer
    {
        private readonly string _boundary;

        private readonly string _boundaryCode;

        private readonly IList<FileData> _multipartFileData;

        private readonly IDictionary<string, object> _multipartFormData;

        public MultiPartStreamer(IDictionary<string, object> multipartFormData, IList<FileData> multipartFileData)
        {
            this._boundaryCode = DateTime.Now.Ticks.GetHashCode() + "548130";
            this._boundary = string.Format("\r\n----------------{0}", this._boundaryCode);

            this._multipartFormData = multipartFormData;
            this._multipartFileData = multipartFileData;
        }

        public void StreamMultiPart(Stream stream)
        {
            stream.WriteString(this._boundary);

            if (this._multipartFormData != null)
            {
                foreach (var entry in this._multipartFormData)
                {
                    stream.WriteString(CreateFormBoundaryHeader(entry.Key, entry.Value));
                    stream.WriteString(this._boundary);
                }
            }

            if (this._multipartFileData != null)
            {
                foreach (var fileData in this._multipartFileData)
                {
                    using (var file = new FileStream(fileData.Filename, FileMode.Open))
                    {
                        stream.WriteString(CreateFileBoundaryHeader(fileData));

                        StreamFileContents(file, fileData, stream);

                        stream.WriteString(this._boundary);
                    }
                }
            }

            stream.WriteString("--");
        }

        private static void StreamFileContents(Stream file, FileData fileData, Stream requestStream)
        {
            var buffer = new byte[8192];

            int count;

            while ((count = file.Read(buffer, 0, buffer.Length)) > 0)
            {
                if (fileData.ContentTransferEncoding == HttpContentTransferEncoding.Base64)
                {
                    string str = Convert.ToBase64String(buffer, 0, count);

                    requestStream.WriteString(str);
                }
                else if (fileData.ContentTransferEncoding == HttpContentTransferEncoding.Binary)
                {
                    requestStream.Write(buffer, 0, count);
                }
            }
        }

        public string GetContentType()
        {
            return string.Format("multipart/form-data; boundary=--------------{0}", this._boundaryCode);
        }

        public long GetContentLength()
        {
            var ascii = new ASCIIEncoding();
            long contentLength = ascii.GetBytes(this._boundary).Length;

            // Multipart Form
            if (this._multipartFormData != null)
            {
                foreach (var entry in this._multipartFormData)
                {
                    contentLength += ascii.GetBytes(CreateFormBoundaryHeader(entry.Key, entry.Value)).Length; // header
                    contentLength += ascii.GetBytes(this._boundary).Length;
                }
            }

            if (this._multipartFileData != null)
            {
                foreach (var fileData in this._multipartFileData)
                {
                    contentLength += ascii.GetBytes(CreateFileBoundaryHeader(fileData)).Length;
                    contentLength += new FileInfo(fileData.Filename).Length;
                    contentLength += ascii.GetBytes(this._boundary).Length;
                }
            }

            contentLength += ascii.GetBytes("--").Length; // ending -- to the boundary

            return contentLength;
        }

        private static string CreateFileBoundaryHeader(FileData fileData)
        {
            return
                string.Format(
                    "\r\nContent-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\n" + "Content-Type: {2}\r\n"
                    + "Content-Transfer-Encoding: {3}\r\n\r\n", 
                    fileData.FieldName, 
                    Path.GetFileName(fileData.Filename), 
                    fileData.ContentType, 
                    fileData.ContentTransferEncoding);
        }

        private static string CreateFormBoundaryHeader(string name, object value)
        {
            return string.Format("\r\nContent-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}", name, value);
        }
    }
}