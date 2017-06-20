#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppenderMediator.cs" company="Leoxia Ltd">
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

using System.Collections.Generic;

#endregion

namespace Leoxia.Log
{
    /// <summary>
    ///     Dispatch log event to all appenders and
    ///     keep track to passed log events for new appenders.
    ///     TODO: remove lock and use Concurrent collection instead
    /// </summary>
    public class AppenderMediator : IAppenderMediator
    {
        // sequence of events by topic
        private readonly List<IAppender> _appenders = new List<IAppender>(10);

        // TODO : by default keep the last X (configurable number) log events.
        private readonly List<ILogEvent> _logEvents = new List<ILogEvent>(10000);

        private readonly object _syncRoot = new object();

        /// <summary>
        ///     Logs the specified log event.
        /// </summary>
        /// <param name="logEvent">The log event.</param>
        public void Log(ILogEvent logEvent)
        {
            IAppender[] appenderArray;
            lock (_syncRoot)
            {
                appenderArray = _appenders.ToArray();
                if (appenderArray.Length == 0) // TODO: should change the behavior by configuration
                {
                    _logEvents.Add(logEvent); // Keep events only if there is no appender yet.
                }
            }
            foreach (var appender in appenderArray)
            {
                appender.Append(logEvent);
            }
        }

        /// <summary>
        ///     Register the subscription of the specified appender.
        /// </summary>
        /// <param name="appender">The appender.</param>
        public void Subscribe(IAppender appender)
        {
            ILogEvent[] logEvents;
            lock (_syncRoot)
            {
                logEvents = _logEvents.ToArray();
                // TODO : alter behavior depending on the configuration
                _logEvents.Clear(); // Clean kept event to avoid memory exhaustion with logs.
                _appenders.Add(appender);
            }
            foreach (var logEvent in logEvents)
            {
                appender.Append(logEvent);
            }
        }

        /// <summary>
        ///     Unsubscribes the specified appender.
        /// </summary>
        /// <param name="appender">The appender.</param>
        public void Unsubscribe(IAppender appender)
        {
            lock (_syncRoot)
            {
                _appenders.Remove(appender);
            }
        }

        /// <summary>
        ///     Clears this instance.
        /// </summary>
        public void Clear()
        {
            lock (_syncRoot)
            {
                _logEvents.Clear();
            }
        }
    }
}