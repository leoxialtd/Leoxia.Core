#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TextWriterAdapterTest.cs" company="Leoxia Ltd">
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

using Leoxia.Testing.IO;
using Moq;
using Xunit;
using Xunit.Abstractions;

#endregion

namespace Leoxia.Testing.Test.IO
{
    public class TextWriterAdapterTest
    {
        private string _capturedText;

        [Fact]
        public void WriteLineTest()
        {
            var outputMock = new Mock<ITestOutputHelper>();
            outputMock.Setup(x => x.WriteLine(It.IsAny<string>())).Callback<string>(CaptureText);
            var output = outputMock.Object;
            var adapter = new TextWriterAdapter(output);
            adapter.WriteLine("Hello World");
            Assert.Equal("Hello World", _capturedText);
            adapter.Write("H");
            adapter.Write('\r');
            adapter.Write('e');
            adapter.Write('l');
            adapter.Write('o');
            adapter.Write('\n');
            Assert.Equal("H\relo", _capturedText);
            adapter.Write('\r');
            adapter.Write('\n');
            Assert.Equal(string.Empty, _capturedText);
        }

        private void CaptureText(string text)
        {
            _capturedText = text;
        }
    }
}