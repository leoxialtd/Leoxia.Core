#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Logger.cs" company="Leoxia Ltd">
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
using System.Diagnostics;
using System.Globalization;
using System.Threading;

#endregion

namespace Leoxia.Log
{
    /// <summary>
    ///     Add a given topic to all messages and transform method calls to LogLevel.
    /// </summary>
    public class Logger : ILogger
    {
        private readonly IAppenderMediator _appenderMediator;
        private readonly ILogEventFactory _factory;
        private readonly string _topic;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Logger" /> class.
        /// </summary>
        /// <param name="appenderMediator">The log event manager.</param>
        /// <param name="factory">The factory.</param>
        /// <param name="topic">The topic.</param>
        public Logger(IAppenderMediator appenderMediator, ILogEventFactory factory, string topic)
        {
            _appenderMediator = appenderMediator;
            _factory = factory;
            _topic = topic;
        }

        /// <summary>
        ///     An Exception has occurred and should be logged.
        /// </summary>
        /// <param name="message">message to log</param>
        /// <param name="exception">exception to log</param>
        public void Exception(string message, Exception exception)
        {
            InnerLog(LogLevel.Error, message + ": " + LogEx.Exception(exception));
        }

        /// <summary>
        ///     An error has occurred and should be logged.
        /// </summary>
        /// <param name="message">message to log</param>
        public void Error(string message)
        {
            InnerLog(LogLevel.Error, message);
        }

        /// <summary>
        ///     Unknown has occurred and a warning should be logged.
        /// </summary>
        /// <param name="message">message to log</param>
        public void Warn(string message)
        {
            InnerLog(LogLevel.Warn, message);
        }

        /// <summary>
        ///     An informative change has occurred and should be notified.
        /// </summary>
        /// <param name="message">message to log</param>
        public void Info(string message)
        {
            InnerLog(LogLevel.Info, message);
        }

        /// <summary>
        ///     A detailed information on the internal behavior is available and could be logged.
        /// </summary>
        /// <param name="message">message to log</param>
        public void Debug(string message)
        {
            if (IsDebugEnabled)
            {
                InnerLog(LogLevel.Debug, message);
            }
        }

        /// <summary>
        ///     An error has occurred and should be logged.
        /// </summary>
        /// <param name="message">message format to log</param>
        /// <param name="parameters">parameters for formatting</param>
        public void ErrorFormat(string message, params object[] parameters)
        {
            InnerFormattedLog(LogLevel.Error, message, parameters);
        }

        /// <summary>
        ///     Unknown has occurred and a warning should be logged.
        /// </summary>
        /// <param name="message">message to log</param>
        /// <param name="parameters">parameters for formatting</param>
        public void WarnFormat(string message, params object[] parameters)
        {
            InnerFormattedLog(LogLevel.Warn, message, parameters);
        }

        /// <summary>
        ///     An informative change has occurred and should be notified.
        /// </summary>
        /// <param name="message">message format to log</param>
        /// <param name="parameters">parameters for formatting</param>
        public void InfoFormat(string message, params object[] parameters)
        {
            InnerFormattedLog(LogLevel.Info, message, parameters);
        }

        /// <summary>
        ///     A detailed information on the internal behavior is available and could be logged.
        /// </summary>
        /// <param name="message">message format to log</param>
        /// <param name="parameters">parameters for formatting</param>
        public void DebugFormat(string message, params object[] parameters)
        {
            if (IsDebugEnabled)
            {
                InnerFormattedLog(LogLevel.Debug, message, parameters);
            }
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is debug enabled.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is debug enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsDebugEnabled { get; set; }

        /// <summary>
        ///     Inner call to do a formatted log.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <param name="message">The message.</param>
        /// <param name="parameters">The parameters.</param>
        private void InnerFormattedLog(LogLevel level, string message, object[] parameters)
        {
            InnerLog(level, string.Format(message, parameters));
        }

        /// <summary>
        ///     Inner call to log a message.
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        /// <param name="message">The message.</param>
        private void InnerLog(LogLevel logLevel, string message)
        {
            _appenderMediator.Log(_factory.Build(logLevel, _topic, message));
        }
    }

    public interface ILogEventFactory
    {
        ILogEvent Build(LogLevel logLevel, string topic, string message);
    }

    public class LogEventFactory : ILogEventFactory
    {
        private static int id;
        private readonly ITimestampProvider _provider;

        public LogEventFactory(ITimestampProvider provider)
        {
            _provider = provider;
        }

        public ILogEvent Build(LogLevel logLevel, string topic, string message)
        {
            var logId = Interlocked.Increment(ref id);
            var stamp = _provider.Now;
            var preciseTimestamp = _provider.PreciseTimestamp;
            var logEvent = new LogEvent(logId, logLevel, topic, message, stamp, preciseTimestamp,
                Thread.CurrentThread.ManagedThreadId.ToString(CultureInfo.InvariantCulture),
                Process.GetCurrentProcess().Id);
            return logEvent;
        }
    }
}