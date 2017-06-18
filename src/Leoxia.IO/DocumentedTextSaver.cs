#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocumentedTextSaver.cs" company="Leoxia Ltd">
//    Copyright (c) 2017 Leoxia Ltd
// </copyright>
// 
// .NET Software Development
// https://www.leoxia.com
// Build. Tomorrow. Together
// 
// MIT License
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
//  --------------------------------------------------------------------------------------------------------------------

#endregion

#region Usings

using System;
using System.Linq;
using System.Text;
using Leoxia.Log;
using Leoxia.Text.Extensions;
using Newtonsoft.Json;

#endregion

namespace Leoxia.IO
{
    /// <summary>
    /// Class in charge of loading and saving a<see cref="DocumentedText{T}"/> from filesystem.
    /// </summary>
    /// <typeparam name="T">type of header</typeparam>
    /// <seealso cref="Leoxia.IO.IDocumentedTextSaver{T}" />
    public class DocumentedTextSaver<T> : IDocumentedTextSaver<T>
        where T : class
    {
        private const string CommentCharacters = "// ";

        private static readonly string DocumentedHeaderEnd =
            CommentCharacters +
            "===== DOCUMENTED HEADER END (DONT EDIT) ======" +
            Environment.NewLine;

        private readonly ILogger _logger = LogManager.GetLogger(typeof(DocumentedTextSaver<T>));
        private readonly ITextAccessor _accessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentedTextSaver{T}"/> class.
        /// </summary>
        /// <param name="accessor">The saver.</param>
        public DocumentedTextSaver(ITextAccessor accessor)
        {
            _accessor = accessor;
        }

        /// <summary>
        /// Saves <see cref="DocumentedText{T}"/> to the specified file path.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <param name="filePath">The file path.</param>
        public void Save(IDocumentedText<T> document, string filePath)
        {
            var builder = new StringBuilder();
            string header = GenerateHeader(document.Header);
            var lines = header.SplitInLines();
            foreach (var line in lines)
            {
                builder.Append(CommentCharacters);
                builder.AppendLine(line);
            }
            builder.Append(DocumentedHeaderEnd);
            builder.Append(document.Content);
            _accessor.Save(filePath, builder.ToString());
        }

        /// <summary>
        /// Loads <see cref="DocumentedText{T}"/> from the specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public IDocumentedText<T> Load(string filePath)
        {
            var text = _accessor.Load(filePath);
            if (!string.IsNullOrEmpty(text))
            {
                var documented = Parse(text);
                return documented;
            }
            return null;
        }

        private IDocumentedText<T> Parse(string text)
        {
            string content;
            var header = ExtractHeader(text, out content);
            var document = new DocumentedText<T>();
            document.Header = ParseHeader(header);
            document.Content = content;
            return document;
        }

        private string GenerateHeader(T documentHeader)
        {
            return JsonConvert.SerializeObject(documentHeader);
        }

        private T ParseHeader(string header)
        {
            var uncommented = header.SplitInLines().Select(x => x.TrimStart(CommentCharacters)).JoinLines();
            return JsonConvert.DeserializeObject<T>(uncommented);
        }

        private string ExtractHeader(string text, out string content)
        {
            var index = text.IndexOf(DocumentedHeaderEnd, StringComparison.Ordinal);
            var header = string.Empty;
            if (index <= 0)
            {
                _logger.Warn("Text is not documented.");
                content = text;
            }
            else
            {
                header = text.Substring(0, index);
                var startIndex = index + DocumentedHeaderEnd.Length;
                content = text.Substring(startIndex, text.Length - startIndex);
            }
            return header;
        }
    }
}