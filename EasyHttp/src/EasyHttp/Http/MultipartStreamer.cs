namespace EasyHttp.Http
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    using EasyHttp.Infrastructure;

    /// <summary>
    /// Class for processing multipart requests/responses
    /// </summary>
    public class MultiPartStreamer
    {
        private readonly string boundary;

        private readonly string boundaryCode;

        private readonly IList<FileData> multipartFileData;

        private readonly IDictionary<string, object> multipartFormData;

        /// <summary>
        /// Class for processing multipart requests/responses
        /// </summary>
        /// <param name="multipartFormData">Data from the multipart request/response</param>
        /// <param name="multipartFileData">File data from the multipart request/response</param>
        public MultiPartStreamer(IDictionary<string, object> multipartFormData, IList<FileData> multipartFileData)
        {
            this.boundaryCode = DateTime.Now.Ticks.GetHashCode() + "548130";
            this.boundary = string.Format("\r\n----------------{0}", this.boundaryCode);

            this.multipartFormData = multipartFormData;
            this.multipartFileData = multipartFileData;
        }

        /// <summary>
        /// Method for setting up the multipart stream
        /// </summary>
        /// <param name="stream">Stream to use for the multipart data</param>
        public void StreamMultiPart(Stream stream)
        {
            stream.WriteString(this.boundary);

            if (this.multipartFormData != null)
            {
                foreach (var entry in this.multipartFormData)
                {
                    stream.WriteString(CreateFormBoundaryHeader(entry.Key, entry.Value));
                    stream.WriteString(this.boundary);
                }
            }

            if (this.multipartFileData != null)
            {
                foreach (var fileData in this.multipartFileData)
                {
                    using (var file = new FileStream(fileData.Filename, FileMode.Open))
                    {
                        stream.WriteString(CreateFileBoundaryHeader(fileData));

                        StreamFileContents(file, fileData, stream);

                        stream.WriteString(this.boundary);
                    }
                }
            }

            stream.WriteString("--");
        }

        /// <summary>
        /// Method for getting the content type.
        /// </summary>
        /// <returns>Content type as string.</returns>
        public string GetContentType()
        {
            return string.Format("multipart/form-data; boundary=--------------{0}", this.boundaryCode);
        }

        /// <summary>
        /// Method for getting the content length.
        /// </summary>
        /// <returns>Content length.</returns>
        public long GetContentLength()
        {
            var ascii = new ASCIIEncoding();
            long contentLength = ascii.GetBytes(this.boundary).Length;

            // Multipart Form
            if (this.multipartFormData != null)
            {
                foreach (var entry in this.multipartFormData)
                {
                    contentLength += ascii.GetBytes(CreateFormBoundaryHeader(entry.Key, entry.Value)).Length; // header
                    contentLength += ascii.GetBytes(this.boundary).Length;
                }
            }

            if (this.multipartFileData != null)
            {
                foreach (var fileData in this.multipartFileData)
                {
                    contentLength += ascii.GetBytes(CreateFileBoundaryHeader(fileData)).Length;
                    contentLength += new FileInfo(fileData.Filename).Length;
                    contentLength += ascii.GetBytes(this.boundary).Length;
                }
            }

            contentLength += ascii.GetBytes("--").Length; // ending -- to the boundary

            return contentLength;
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

        /// <summary>
        /// Method for creating File Boundary Header.
        /// </summary>
        /// <param name="fileData">File data.</param>
        /// <returns>Boundary header for the file data.</returns>
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

        /// <summary>
        /// Method for creating form boundary header. 
        /// </summary>
        /// <param name="name">Form-data name.</param>
        /// <param name="value">Form- data value.</param>
        /// <returns></returns>
        private static string CreateFormBoundaryHeader(string name, object value)
        {
            return string.Format("\r\nContent-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}", name, value);
        }
    }
}