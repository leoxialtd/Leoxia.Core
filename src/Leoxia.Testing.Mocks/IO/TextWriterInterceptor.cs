#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextWriterInterceptor.cs" company="Leoxia Ltd">
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

#endregion

namespace Leoxia.Testing.IO
{
    public class TextWriterInterceptor : TextWriter
    {
        private readonly StringBuilder _builder = new StringBuilder();

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.IO.TextWriter" /> class.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public TextWriterInterceptor(TextWriter writer)
        {
            Inner = writer;
        }

        /// <summary>When overridden in a derived class, returns the character encoding in which the output is written.</summary>
        /// <returns>The character encoding in which the output is written.</returns>
        public override Encoding Encoding => Inner.Encoding;

        /// <summary>
        ///     Gets the inner writer.
        /// </summary>
        /// <value>
        ///     The inner writer.
        /// </value>
        public TextWriter Inner { get; }

        /// <summary>
        ///     Gets the intercepted text.
        /// </summary>
        /// <value>
        ///     The intercepted text.
        /// </value>
        public string InterceptedText => _builder.ToString();

        /// <summary>Writes a character to the text string or stream.</summary>
        /// <param name="value">The character to write to the text stream. </param>
        /// <exception cref="T:System.ObjectDisposedException">The <see cref="T:System.IO.TextWriter" /> is closed. </exception>
        /// <exception cref="T:System.IO.IOException">An I/O error occurs. </exception>
        public override void Write(char value)
        {
            _builder.Append(value);
            Inner.Write(value);
        }
    }
}