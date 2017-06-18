#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocumentedTextSaverTest.cs" company="Leoxia Ltd">
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
using Moq;
using Xunit;

#endregion

namespace Leoxia.IO.Test
{
    public class DocumentedTextSaverTest
    {
        private string _capturedText;

        [Fact]
        public void UseCase()
        {
            var textSaverMock = new Mock<ITextAccessor>();
            textSaverMock.Setup(x => x.Save(It.IsAny<string>(), It.IsAny<string>()))
                .Callback((Action<string, string>) CaptureText);
            textSaverMock.Setup(x => x.Load(It.IsAny<string>())).Returns(() => _capturedText);
            var textSaver = textSaverMock.Object;
            var documentedText = new DocumentedText<Header>();
            documentedText.Header = new Header {Time = DateTime.Now};
            documentedText.Content = "MyContent";
            var saver = new DocumentedTextSaver<Header>(textSaver);
            saver.Save(documentedText, "fileName");
            var res = saver.Load("fileName");
            Assert.NotNull(res);
            Assert.Equal(documentedText.Content, res.Content);
            Assert.Equal(documentedText.Header.Time, res.Header.Time);
        }

        private void CaptureText(string arg1, string arg2)
        {
            _capturedText = arg2;
        }
    }

    public class Header
    {
        public DateTime Time { get; set; }
    }
}