#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SingleWriterProvider.cs" company="Leoxia Ltd">
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
using Leoxia.Abstractions.IO;

#endregion

namespace Leoxia.Testing.IO
{
    /// <summary>
    ///     Provider giving the same <see cref="TextWriter" /> for output and error.
    /// </summary>
    /// <seealso cref="Leoxia.Abstractions.IO.IStandardWriterProvider" />
    public class SingleWriterProvider : IStandardWriterProvider
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SingleWriterProvider" /> class.
        /// </summary>
        /// <param name="writer">The writer.</param>
        public SingleWriterProvider(TextWriter writer)
        {
            Out = writer;
        }

        /// <summary>
        ///     Gets or sets the output.
        /// </summary>
        /// <value>
        ///     The out.
        /// </value>
        public TextWriter Out { get; }

        /// <summary>
        ///     Gets or sets the error output.
        /// </summary>
        /// <value>
        ///     The error.
        /// </value>
        public TextWriter Error => Out;
    }
}