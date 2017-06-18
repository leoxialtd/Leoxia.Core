#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileAppenderTests.cs" company="Leoxia Ltd">
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
using System.Linq;
using System.Text;
using System.Threading;
using Leoxia.Abstractions;
using Leoxia.Log.IO;
using Leoxia.Testing.Reflection;
using Moq;
using Xunit;

#endregion

namespace Leoxia.Log.Tests.IO
{
    public static class Constants
    {
        public const string TimeProviderFieldName = "_timeProvider";
    }

    public class FileAppenderTests
    {
        private readonly DateTime _date = DateTime.Now;
        private readonly string _dateString;

        public FileAppenderTests()
        {
            var mockProvider = new Mock<ITimeProvider>();
            mockProvider.Setup(x => x.Now).Returns(_date);
            _dateString = _date.ToString("dd/MM/yyyy HH:mm:ss,fff");
        }

        [Fact]
        public void AppendShouldProduceLog()
        {
            var file = "file1.txt";
            var appender = new FileAppender(file);
            var streamFactory = new Mock<IStreamFactory>();
            var stream = new MemoryStream();
            var streamWriter = new StreamWriter(stream);
            streamFactory.Setup(x => x.CreateStreamWriter(It.IsAny<string>())).Returns(streamWriter);
            appender.SetField("_factory", streamFactory.Object);
            appender.Append(BuidLogEvent("My Topic", "My Message"));
            streamWriter.Flush();

            var res = GetResult(stream);
            var expected = _dateString
                           + " - [Info] My Topic: My Message ["
                           + Thread.CurrentThread.ManagedThreadId + "]"
                           + Environment.NewLine;
            Assert.Equal(expected, res);

            appender.Append(BuidLogEvent("My Topic2", "My Message2"));
            streamWriter.Flush();
            res = GetResult(stream);
            appender.Dispose();
            expected = _dateString
                       + " - [Info] My Topic2: My Message2 ["
                       + Thread.CurrentThread.ManagedThreadId + "]"
                       + Environment.NewLine;
            Assert.Equal(expected, res);
        }

        private LogEvent BuidLogEvent(string topic, string message)
        {
            return new LogEvent(0, LogLevel.Info, topic, message, _date, 0, Thread.CurrentThread.ManagedThreadId.ToString(), 1);
        }

        private static string GetResult(MemoryStream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            var buffer = stream.ToArray();
            return Encoding.UTF8.GetString(buffer).TakeWhile(x => x != '\0').Aggregate("", (x, y) => x + y);
        }

        [Fact]
        public void IntegrationTestWithFileShouldProduceExpectedResult()
        {
            var file = "file.txt";
            var appender = new FileAppender(file);
            appender.Append(BuidLogEvent("My Topic", "My Message"));
            appender.Dispose();
            var reader = File.OpenText(file);
            var result = reader.ReadToEnd();
            var expected = _dateString
                           + " - [Info] My Topic: My Message ["
                           + Thread.CurrentThread.ManagedThreadId + "]"
                           + Environment.NewLine;
            Assert.Equal(expected, result);
        }
    }
}