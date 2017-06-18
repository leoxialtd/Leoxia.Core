#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppenderMediatorTests.cs" company="Leoxia Ltd">
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

namespace Leoxia.Log.Tests
{
    public class AppenderMediatorTests
    {
        private LogEvent _logEvent;
        private LogEvent _logEvent2;

        [Fact]
        public void LogALogEventShouldAppendItToExistingAppender()
        {
            var mediator = new AppenderMediator();
            var appender = new Mock<IAppender>();
            mediator.Subscribe(appender.Object);
            _logEvent = new LogEvent(0, LogLevel.Error, "topic", "message", DateTime.Now, 0, "1", 1);
            mediator.Log(_logEvent);
            appender.Verify(x => x.Append(_logEvent));
            appender.VerifyAll();
        }

        [Fact]
        public void LogALogEventShouldNotAppendItToRemovedAppender()
        {
            var mediator = new AppenderMediator();
            var appender = new Mock<IAppender>(MockBehavior.Strict);
            mediator.Subscribe(appender.Object);
            mediator.Unsubscribe(appender.Object);
            _logEvent = BuildLogEvent();
            mediator.Log(_logEvent);
            appender.VerifyAll();
        }

        private static LogEvent BuildLogEvent()
        {
            return new LogEvent(0, LogLevel.Error, "topic", "message", DateTime.Now, 0, "1", 1);
        }

        [Fact]
        public void AddAnAppenderShouldLogAnyPreviousLogEvent()
        {
            var mediator = new AppenderMediator();
            _logEvent = BuildLogEvent();
            _logEvent2 = BuildLogEvent();
            mediator.Log(_logEvent);
            mediator.Log(_logEvent2);
            var appender = new Mock<IAppender>();
            mediator.Subscribe(appender.Object);
            appender.Verify(x => x.Append(_logEvent));
            appender.Verify(x => x.Append(_logEvent2));
            appender.VerifyAll();
        }
    }
}