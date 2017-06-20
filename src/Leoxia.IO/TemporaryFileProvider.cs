#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TemporaryFileProvider.cs" company="Leoxia Ltd">
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

#endregion

namespace Leoxia.IO
{
    /// <summary>
    ///     Provide temporary file path as a file in a temporary directory.
    /// </summary>
    /// <seealso cref="Leoxia.IO.ITemporaryFileProvider" />
    public class TemporaryFileProvider : ITemporaryFileProvider
    {
        private readonly IDirectoryInfoProvider _provider;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TemporaryFileProvider" /> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        public TemporaryFileProvider(IDirectoryInfoProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        ///     Gets the specified temporary file path.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>
        ///     temporary file path
        /// </returns>
        public string Get(string fileName)
        {
            var directory = _provider.Get();
            if (directory == null)
            {
                return fileName;
            }
            return Path.Combine(directory.FullName, fileName);
        }
    }
}