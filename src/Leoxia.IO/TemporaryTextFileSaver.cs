#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TemporaryTextFileSaver.cs" company="Leoxia Ltd">
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

using System.IO;
using Leoxia.Log;

#endregion

namespace Leoxia.IO
{
    /// <summary>
    ///     Purpose here is to save temporary generated files
    ///     for human analysis
    /// </summary>
    public class TemporaryTextFileAccessor : ITextAccessor
    {
        private readonly ILogger _logger = LogManager.GetLogger(typeof(TemporaryTextFileAccessor));
        private readonly ITemporaryFileProvider _provider;

        public TemporaryTextFileAccessor(ITemporaryFileProvider temporaryFileProvider)
        {
            _provider = temporaryFileProvider;
        }

        /// <summary>
        /// Saves text to the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="content">The content.</param>
        public void Save(string fileName, string content)
        {
            var filePath = _provider.Get(fileName);
            _logger.Debug($"Saving generated file in {filePath}");
            using (var writer = File.CreateText(filePath))
            {
                writer.Write(content);
            }
        }

        /// <summary>
        /// Loads text from the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>text</returns>
        public string Load(string fileName)
        {
            var filePath = _provider.Get(fileName);
            using (var file = File.OpenText(filePath))
            {
                return file.ReadToEnd();
            }
        }

        /// <summary>
        /// Returns whether the specified file name exists.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public bool Exists(string fileName)
        {
            var filePath = _provider.Get(fileName);
            return File.Exists(filePath);
        }
    }
}