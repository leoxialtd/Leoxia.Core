#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnvironmentDirectoryInfoProvider.cs" company="Leoxia Ltd">
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

using Leoxia.Abstractions;
using Leoxia.Abstractions.IO;

#endregion

namespace Leoxia.IO
{
    /// <summary>
    ///     Get a directory path from <see cref="IEnvironment" />
    /// </summary>
    /// <seealso cref="Leoxia.IO.IDirectoryInfoProvider" />
    public class EnvironmentDirectoryInfoProvider : IDirectoryInfoProvider
    {
        private readonly IDirectory _directoryFileSystem;
        private readonly IEnvironment _environment;
        private readonly string _environmentVariableKey;
        private readonly IDirectoryInfoFactory _factory;

        /// <summary>
        ///     Initializes a new instance of the <see cref="EnvironmentDirectoryInfoProvider" /> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="environmentVariableKey">The environment variable key.</param>
        /// <param name="factory">The factory.</param>
        /// <param name="directoryFileSystem">The directory file system.</param>
        public EnvironmentDirectoryInfoProvider(
            IEnvironment environment,
            string environmentVariableKey,
            IDirectoryInfoFactory factory,
            IDirectory directoryFileSystem)
        {
            _environment = environment;
            _environmentVariableKey = environmentVariableKey;
            _factory = factory;
            _directoryFileSystem = directoryFileSystem;
        }

        /// <summary>
        ///     Gets a <see cref="IDirectoryInfo" /> from environment.
        /// </summary>
        /// <returns></returns>
        public IDirectoryInfo Get()
        {
            var configurationRoot = _environment.GetEnvironmentVariable(_environmentVariableKey);
            if (string.IsNullOrEmpty(configurationRoot))
            {
                return _factory.Build(_directoryFileSystem.GetCurrentDirectory());
            }
            return _factory.Build(configurationRoot);
        }
    }
}