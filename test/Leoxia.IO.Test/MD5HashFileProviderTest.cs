#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MD5HashFileProviderTest.cs" company="Leoxia Ltd">
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
using Leoxia.Abstractions.IO;
using Moq;
using Xunit;

#endregion

namespace Leoxia.IO.Test
{
    // ReSharper disable once InconsistentNaming
    public class MD5HashFileProviderTest
    {
        [Fact]
        public void GetHashTest()
        {
            var provider = new MD5HashFileProvider();
            var buffer = Encoding.UTF8.GetBytes("Hello World");
            var stream = new MemoryStream(buffer);
            var fileInfoMock = new Mock<IFileInfo>();
            fileInfoMock.SetupGet(x => x.Exists).Returns(true);
            fileInfoMock.Setup(x => x.OpenRead()).Returns(stream);
            var fileInfo = fileInfoMock.Object;
            var hash = provider.GetFileHash(fileInfo);
            Assert.NotNull(hash);
            Assert.NotEmpty(hash);
            var buffer2 = Encoding.UTF8.GetBytes("Hello World");
            var stream2 = new MemoryStream(buffer2);
            var fileInfoMock2 = new Mock<IFileInfo>();
            fileInfoMock2.Setup(x => x.OpenRead()).Returns(stream2);
            fileInfoMock2.SetupGet(x => x.Exists).Returns(true);
            var hash2 = provider.GetFileHash(fileInfoMock2.Object);
            Assert.Equal(hash, hash2);
            var buffer3 = Encoding.UTF8.GetBytes("Hello World2");
            var stream3 = new MemoryStream(buffer3);
            var fileInfoMock3 = new Mock<IFileInfo>();
            fileInfoMock3.SetupGet(x => x.Exists).Returns(true);
            fileInfoMock3.Setup(x => x.OpenRead()).Returns(stream3);
            var hash3 = provider.GetFileHash(fileInfoMock3.Object);
            Assert.NotEqual(hash, hash3);
        }
    }
}