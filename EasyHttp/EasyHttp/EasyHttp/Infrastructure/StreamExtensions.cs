namespace EasyHttp.Infrastructure
{
    using System.IO;
    using System.Text;

    public static class StreamExtensions
    {
        /// <summary>
        /// Method for writing the value of a string after it is encoded as a sequence of bytes.
        /// </summary>
        /// <param name="stream">The stream which will write the encoded string.</param>
        /// <param name="value">The string which will be encoded.</param>
        public static void WriteString(this Stream stream, string value)
        {
            var buffer = Encoding.ASCII.GetBytes(value);

            stream.Write(buffer, 0, buffer.Length);
        }
    }
}