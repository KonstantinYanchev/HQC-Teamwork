﻿#region License

// Distributed under the BSD License
// =================================
// Copyright (c) 2010, Hadi Hariri
// All rights reserved.
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//     * Redistributions of source code must retain the above copyright
//       notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
//     * Neither the name of Hadi Hariri nor the
//       names of its contributors may be used to endorse or promote products
//       derived from this software without specific prior written permission.
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// =============================================================
// 
// Parts of this Software use JsonFX Serialization Library which is distributed under the MIT License:
// Distributed under the terms of an MIT-style license:
// The MIT License
// Copyright (c) 2006-2009 Stephen M. McKamey
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
#endregion

namespace EasyHttp.Codecs.JsonFXExtensions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    using JsonFx.Serialization;
    using JsonFx.Serialization.Providers;

    /// <summary>
    ///  Data writer provider that uses regular expressions.
    /// </summary>
    public class RegExBasedDataWriterProvider : IDataWriterProvider
    {
        private readonly IDataWriter defaultWriter;

        private readonly IDictionary<string, IDataWriter> writersByExt =
            new Dictionary<string, IDataWriter>(StringComparer.OrdinalIgnoreCase);

        private readonly IDictionary<string, IDataWriter> writersByMime =
            new Dictionary<string, IDataWriter>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Data writer provider which uses Regullar Expressions.
        /// </summary>
        /// <param name="writers">Collection of DataWriters that will be added to the class by Mime Type or by File Extention.</param>
        public RegExBasedDataWriterProvider(IEnumerable<IDataWriter> writers)
        {
            if (writers != null)
            {
                foreach (var writer in writers)
                {
                    if (this.defaultWriter == null)
                    {
                        this.defaultWriter = writer;
                    }

                    foreach (var contentType in writer.ContentType)
                    {
                        if (string.IsNullOrEmpty(contentType) || this.writersByMime.ContainsKey(contentType))
                        {
                            continue;
                        }

                        this.writersByMime[contentType] = writer;
                    }

                    foreach (var fileExt in writer.FileExtension)
                    {
                        if (string.IsNullOrEmpty(fileExt) || this.writersByExt.ContainsKey(fileExt))
                        {
                            continue;
                        }

                        string ext = NormalizeExtension(fileExt);
                        this.writersByExt[ext] = writer;
                    }
                }
            }
        }

        /// <summary>
        ///  Dafault Data Writer.
        /// </summary>
        public IDataWriter DefaultDataWriter
        {
            get
            {
                return this.defaultWriter;
            }
        }

        /// <summary>
        /// Parse strings to get mime types out of them.
        /// </summary>
        /// <param name="accept">String containing all acceptable content types.</param>
        /// <param name="contentType">String from which to get the content type.</param>
        /// <returns>Collection of mime types.</returns>
        public static IEnumerable<string> ParseHeaders(string accept, string contentType)
        {
            string mime;

            // check for a matching accept type
            foreach (var type in SplitTrim(accept, ','))
            {
                mime = DataProviderUtility.ParseMediaType(type);
                if (!string.IsNullOrEmpty(mime))
                {
                    yield return mime;
                }
            }

            // fallback on content-type
            mime = DataProviderUtility.ParseMediaType(contentType);
            if (!string.IsNullOrEmpty(mime))
            {
                yield return mime;
            }
        }

        /// <summary>
        /// Get a data reader by file extention.
        /// </summary>
        /// <param name="extension">File extention by which to get the data provider</param>
        /// <returns>Data Writer that corresponds to the required file extention, if such exist. Otherwise returns null</returns>
        public IDataWriter Find(string extension)
        {
            extension = NormalizeExtension(extension);

            IDataWriter writer;
            if (this.writersByExt.TryGetValue(extension, out writer))
            {
                return writer;
            }

            return null;
        }

        /// <summary>
        /// Get a data reader by content type.
        /// </summary>
        /// <param name="acceptHeader">String containing all acceptable content types.</param>
        /// <param name="contentTypeHeader">String from which to get the content type.</param>
        /// <returns>DataWriter that corresponds to the required content type if such exist. Otherwise returns null.</returns>
        public IDataWriter Find(string acceptHeader, string contentTypeHeader)
        {
            foreach (var type in ParseHeaders(acceptHeader, contentTypeHeader))
            {
                var readers = from writer in this.writersByMime
                              where Regex.Match(type, writer.Key, RegexOptions.Singleline).Success
                              select writer;

                if (readers.Count() > 0)
                {
                    return readers.First().Value;
                }
            }

            return null;
        }

        private static IEnumerable<string> SplitTrim(string source, char ch)
        {
            if (string.IsNullOrEmpty(source))
            {
                yield break;
            }

            int length = source.Length;
            for (int prev = 0, next = 0; prev < length && next >= 0; prev = next + 1)
            {
                next = source.IndexOf(ch, prev);
                if (next < 0)
                {
                    next = length;
                }

                string part = source.Substring(prev, next - prev).Trim();
                if (part.Length > 0)
                {
                    yield return part;
                }
            }
        }

        private static string NormalizeExtension(string extension)
        {
            if (string.IsNullOrEmpty(extension))
            {
                return string.Empty;
            }

            // ensure is only extension with leading dot
            return Path.GetExtension(extension);
        }
    }
}