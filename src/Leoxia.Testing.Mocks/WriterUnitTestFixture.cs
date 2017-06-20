#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WriterUnitTestFixture.cs" company="Leoxia Ltd">
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

using DryIoc;
using Leoxia.Abstractions.IO;
using Leoxia.Testing.Mocks.IO;
using Xunit.Abstractions;

#endregion

namespace Leoxia.Testing.Mocks
{
    /// <summary>
    ///     Unit test fixture providing writer out and writer error for tests
    ///     with text interception like console testing.
    /// </summary>
    /// <seealso cref="MockUnitTestFixture" />
    public class WriterUnitTestFixture : MockUnitTestFixture
    {
        private readonly TextWriterInterceptor _interceptor;

        /// <summary>
        ///     Initializes a new instance of the <see cref="WriterUnitTestFixture" /> class.
        /// </summary>
        /// <param name="output">The output.</param>
        public WriterUnitTestFixture(ITestOutputHelper output)
        {
            Output = output;
            var adapter = new TextWriterAdapter(output);
            _interceptor = new TextWriterInterceptor(adapter);
            WriterProvider = new SingleWriterProvider(_interceptor);
            Container.RegisterInstance(WriterProvider);
        }

        /// <summary>
        ///     Gets the intercepted text.
        /// </summary>
        /// <value>
        ///     The intercepted text.
        /// </value>
        public string InterceptedText => _interceptor.InterceptedText;

        /// <summary>
        ///     Gets the output.
        /// </summary>
        /// <value>
        ///     The output.
        /// </value>
        public ITestOutputHelper Output { get; }

        /// <summary>
        ///     Gets the writer provider.
        /// </summary>
        /// <value>
        ///     The writer provider.
        /// </value>
        public IStandardWriterProvider WriterProvider { get; }
    }
}