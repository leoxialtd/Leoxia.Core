#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConsoleAppenderTests.cs" company="Leoxia Ltd">
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
using System.Threading;
using Leoxia.Log.IO;
using Leoxia.Testing.Assertions;
using Leoxia.Testing.Reflection;
using Moq;
using Xunit;

#endregion

namespace Leoxia.Log.Tests.IO
{
    public class ConsoleAppenderTests
    {
        private string _lastLine;
        private Mock<IConsoleWrapper> _wrapper;

        [Fact]
        public void UseCase()
        {
            var appender = new ConsoleAppender();
            _wrapper = new Mock<IConsoleWrapper>();
            _wrapper.Setup(x => x.WriteLine(It.IsAny<string>())).Callback<string>(x => _lastLine = x);

            appender.SetFirstField(_wrapper.Object);
            appender.Append(new LogEvent(0, LogLevel.Info, "A topic", "Some Info", DateTime.Now, 0,
                Thread.CurrentThread.ManagedThreadId.ToString(), 1));
            var end = " - [Info] A topic: Some Info [" + Thread.CurrentThread.ManagedThreadId + "]";
            Check.That(_lastLine.EndsWith(end))
                .IsTrue(_lastLine + " should end with " + end);
        }
    }
}