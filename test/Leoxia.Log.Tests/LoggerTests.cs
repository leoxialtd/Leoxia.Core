#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoggerTests.cs" company="Leoxia Ltd">
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
using System.Globalization;
using System.Threading;
using Leoxia.Abstractions;
using Leoxia.Testing.Assertions;
using Moq;
using Xunit;

#endregion

namespace Leoxia.Log.Tests
{
    public class LoggerTests
    {
        private readonly DateTime _date = DateTime.Now;
        private readonly ILogger _logger;
        private ILogEvent _lastEvent;

        public LoggerTests()
        {
            var mockProvider = new Mock<ITimeProvider>();
            mockProvider.Setup(x => x.Now).Returns(_date);
            _lastEvent = null;
            var appenderMediatorMock = new Mock<IAppenderMediator>();
            appenderMediatorMock.Setup(x => x.Log(It.IsAny<ILogEvent>())).Callback<ILogEvent>(x => SetLogEvent(x));
            var appenderMediator = appenderMediatorMock.Object;
            _logger = new Logger(appenderMediator, new LogEventFactory(new TimestampProvider(mockProvider.Object)),
                "Topic");
        }

        private void SetLogEvent(ILogEvent x)
        {
            _lastEvent = x;
        }

        [Fact]
        public void UseCaseError()
        {
            _logger.Error("ErrorMessage");
            Assert.NotNull(_lastEvent);
            Assert.Equal(LogLevel.Error, _lastEvent.Level);
            Assert.Equal("ErrorMessage", _lastEvent.Message);
            CheckThreadId();
            Assert.Equal("Topic", _lastEvent.Topic);
            CheckDate();
        }

        [Fact]
        public void UseCaseException()
        {
            var exception = new Exception();
            _logger.Exception("ExMessage", exception);
            Assert.NotNull(_lastEvent);
            Assert.Equal(LogLevel.Error, _lastEvent.Level);
            Assert.Equal("ExMessage: " + exception + Environment.NewLine, _lastEvent.Message);
            CheckThreadId();
            Assert.Equal("Topic", _lastEvent.Topic);
            CheckDate();
        }

        [Fact]
        public void UseCaseInnerException()
        {
            var argumentNullException = new ArgumentNullException();
            var exception = new Exception("message", argumentNullException);
            _logger.Exception("ExMessage", exception);
            Assert.NotNull(_lastEvent);
            Assert.Equal(LogLevel.Error, _lastEvent.Level);
            Assert.Equal("ExMessage: " + argumentNullException + Environment.NewLine
                         + exception + Environment.NewLine, _lastEvent.Message);
            CheckThreadId();
            Assert.Equal("Topic", _lastEvent.Topic);
            CheckDate();
        }

        private void CheckDate()
        {
            Check.That(_lastEvent.Date).IsEqualTo(_date,
                "Timestamp is not the mocked one, maybe replacement of provider by reflection was broken");
        }

        [Fact]
        public void UseCaseErrorFormat()
        {
            _logger.ErrorFormat("{1} ErrorMessage {0}", "FooBar", "BarFoo");
            Assert.NotNull(_lastEvent);
            Assert.Equal(LogLevel.Error, _lastEvent.Level);
            Assert.Equal("BarFoo ErrorMessage FooBar", _lastEvent.Message);
            CheckThreadId();
            Assert.Equal("Topic", _lastEvent.Topic);
            CheckDate();
        }

        private void CheckThreadId()
        {
            Assert.Equal(Thread.CurrentThread.ManagedThreadId.ToString(CultureInfo.InvariantCulture),
                _lastEvent.ThreadId);
        }


        [Fact]
        public void UseCaseDebug()
        {
            _logger.IsDebugEnabled = true;
            _logger.Debug("DebugMessage");
            Assert.NotNull(_lastEvent);
            Assert.Equal(LogLevel.Debug, _lastEvent.Level);
            Assert.Equal("DebugMessage", _lastEvent.Message);
            CheckThreadId();
            CheckDate();
        }

        [Fact]
        public void UseCaseDebugFormatWhenDebugEnabled()
        {
            _logger.IsDebugEnabled = true;
            _logger.DebugFormat("{0} DebugMessage {1}", "FooBar", "BarFoo");
            Assert.NotNull(_lastEvent);
            Assert.Equal(LogLevel.Debug, _lastEvent.Level);
            Assert.Equal("FooBar DebugMessage BarFoo", _lastEvent.Message);
            CheckThreadId();
            CheckDate();
        }

        [Fact]
        public void UseCaseDebugFormatWhenDebugDisabled()
        {
            _logger.IsDebugEnabled = false;
            _logger.DebugFormat("{0} DebugMessage {1}", "FooBar", "BarFoo");
            Assert.Null(_lastEvent);
        }


        [Fact]
        public void UseCaseWarn()
        {
            _logger.Warn("WarnMessage");
            Assert.NotNull(_lastEvent);
            Assert.Equal(LogLevel.Warn, _lastEvent.Level);
            Assert.Equal("WarnMessage", _lastEvent.Message);
            CheckThreadId();
            CheckDate();
        }

        [Fact]
        public void UseCaseWarnFormat()
        {
            _logger.WarnFormat("{0} WarnMessage {1}", "FooBar", "BarFoo");
            Assert.NotNull(_lastEvent);
            Assert.Equal(LogLevel.Warn, _lastEvent.Level);
            Assert.Equal("FooBar WarnMessage BarFoo", _lastEvent.Message);
            CheckThreadId();
            CheckDate();
        }

        [Fact]
        public void UseCaseInfo()
        {
            _logger.Info("InfoMessage");
            Assert.NotNull(_lastEvent);
            Assert.Equal(LogLevel.Info, _lastEvent.Level);
            Assert.Equal("InfoMessage", _lastEvent.Message);
            CheckThreadId();
            CheckDate();
        }


        [Fact]
        public void UseCaseInfoFormat()
        {
            _logger.InfoFormat("{0} InfoMessage {1}", "FooBar", "BarFoo");
            Assert.NotNull(_lastEvent);
            Assert.Equal(LogLevel.Info, _lastEvent.Level);
            Assert.Equal("FooBar InfoMessage BarFoo", _lastEvent.Message);
            CheckThreadId();
            CheckDate();
        }
    }
}