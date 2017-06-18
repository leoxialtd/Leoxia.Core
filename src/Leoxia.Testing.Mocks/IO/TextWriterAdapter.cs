#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextWriterAdapter.cs" company="Leoxia Ltd">
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
using System.Text;
using Xunit.Abstractions;

#endregion

namespace Leoxia.Testing.IO
{
    /// <summary>
    ///     Adapter for XUnit output helper into TextWriter
    /// </summary>
    /// <seealso cref="System.IO.TextWriter" />
    public class TextWriterAdapter : TextWriter
    {
        private readonly ITestOutputHelper _output;
        private readonly object _synchro = new object();
        private StringBuilder _builder = new StringBuilder();
        private bool _lastReturn;

        /// <summary>Initializes a new instance of the <see cref="T:System.IO.TextWriter" /> class.</summary>
        public TextWriterAdapter(ITestOutputHelper output)
        {
            _output = output;
        }

        /// <summary>When overridden in a derived class, returns the character encoding in which the output is written.</summary>
        /// <returns>The character encoding in which the output is written.</returns>
        public override Encoding Encoding => Encoding.UTF8;

        /// <summary>Writes a character to the text string or stream.</summary>
        /// <param name="value">The character to write to the text stream. </param>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        public override void Write(char value)
        {
            lock (_synchro)
            {
                if (value == '\n')
                {
                    _output.WriteLine(_builder.ToString());
                    _builder = new StringBuilder();
                }
                else
                {
                    if (_lastReturn)
                    {
                        _builder.Append('\r');
                    }
                    if (value == '\r')
                    {
                        _lastReturn = true;
                        return;
                    }
                    _builder.Append(value);
                }
                _lastReturn = false;
            }
        }
    }
}