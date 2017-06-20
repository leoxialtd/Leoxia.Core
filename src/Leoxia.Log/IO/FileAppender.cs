#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileAppender.cs" company="Leoxia Ltd">
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
using System.IO;

#endregion

namespace Leoxia.Log.IO
{
    /// <summary>
    ///     Append logs into a file.
    /// </summary>
    /// <seealso cref="Leoxia.Log.IAppender" />
    /// <seealso cref="System.IDisposable" />
    public sealed class FileAppender : IAppender, IDisposable
    {
        private readonly IStreamFactory _factory = new FileStreamFactory();
        private readonly string _file;
        private readonly LogFormatter _logFormatter = new LogFormatter();
        private readonly ILogFormatProvider _provider;
        private StreamWriter _writer;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FileAppender" /> class.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="provider">The provider.</param>
        public FileAppender(string file, ILogFormatProvider provider = null)
        {
            if (!Path.IsPathRooted(file))
            {
                // HACK: We do that to avoid streamwriterfactory to create file 
                // where the System.IO nuget package cache location... 
                // Depends on the test runner
                file = Path.Combine(Directory.GetCurrentDirectory(), file);
            }
            _file = file;
            if (provider == null)
            {
                provider = DefaultLogFormatProvider.Instance;
            }
            _provider = provider;
        }

        /// <summary>
        ///     Appends the specified log event.
        /// </summary>
        /// <param name="logEvent">The log event.</param>
        public void Append(ILogEvent logEvent)
        {
            if (_writer == null)
            {
                _writer = _factory.CreateStreamWriter(_file);
                _writer.AutoFlush = true;
            }
            _writer.WriteLine(_logFormatter.Format(_provider, logEvent));
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_writer != null)
            {
                _writer.Flush();
                _writer.Dispose();
            }
        }
    }
}