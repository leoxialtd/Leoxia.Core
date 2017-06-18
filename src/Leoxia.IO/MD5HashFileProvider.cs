#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MD5HashFileProvider.cs" company="Leoxia Ltd">
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

using System;
using System.Security.Cryptography;
using Leoxia.Abstractions.IO;

namespace Leoxia.IO
{
    /// <summary>
    ///     Provide MD5 Hash of a file given a <see cref="IFileInfo" /> on it.
    /// </summary>
    /// <seealso cref="Leoxia.IO.IHashFileProvider" />
    // ReSharper disable once InconsistentNaming
    public class MD5HashFileProvider : IHashFileProvider
    {
        /// <summary>
        /// Gets the <see cref="IFileInfo" /> hash.
        /// </summary>
        /// <param name="fileInfo">The file information.</param>
        /// <returns>
        ///   <see cref="string" /> hash
        /// </returns>
        public string GetFileHash(IFileInfo fileInfo)
        {
            if (fileInfo.Exists)
            {
                using (var md5 = MD5.Create())
                {
                    using (var stream = fileInfo.OpenRead())
                    {
                        return Convert.ToBase64String(md5.ComputeHash(stream));
                    }
                }
            }
            return null;
        }
    }
}