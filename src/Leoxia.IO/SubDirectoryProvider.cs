#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SubDirectoryProvider.cs" company="Leoxia Ltd">
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
using System.IO;
using System.Linq;
using Leoxia.Abstractions.IO;

namespace Leoxia.IO
{
    /// <summary>
    /// Provides <see cref="IDirectoryInfo" /> as a sub directory of a reference one.
    /// </summary>
    /// <seealso cref="Leoxia.IO.IDirectoryInfoProvider" />
    public class SubDirectoryProvider : IDirectoryInfoProvider
    {
        private readonly IDirectoryInfo _directoryPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubDirectoryProvider"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="subDirectory">The sub directory.</param>
        public SubDirectoryProvider(IDirectoryInfoProvider provider, string subDirectory = null) :
            this(provider.Get(), subDirectory)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SubDirectoryProvider"/> class.
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <param name="subDirectory">The sub directory.</param>
        /// <exception cref="ArgumentNullException">directoryPath</exception>
        public SubDirectoryProvider(IDirectoryInfo directoryPath, string subDirectory = null)
        {
            if (directoryPath == null)
            {
                throw new ArgumentNullException(nameof(directoryPath));
            }
            _directoryPath = GetDirectoryPath(directoryPath, subDirectory);
        }

        /// <summary>
        /// Gets <see cref="IDirectoryInfo" />.
        /// </summary>
        /// <returns></returns>
        public IDirectoryInfo Get()
        {
            return _directoryPath;
        }

        private IDirectoryInfo GetDirectoryPath(IDirectoryInfo directoryPath, string subDirectory)
        {
            if (string.IsNullOrEmpty(subDirectory))
            {
                return directoryPath;
            }
            IDirectoryInfo target = null;
            target = directoryPath.GetDirectories(subDirectory, SearchOption.TopDirectoryOnly).FirstOrDefault();
            if (target == null || !target.Exists)
            {
                target = directoryPath.CreateSubdirectory(subDirectory);
            }
            return target;
        }
    }
}