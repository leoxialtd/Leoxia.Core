#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogEventFactory.cs" company="Leoxia Ltd">
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

using System.Diagnostics;
using System.Globalization;
using System.Threading;

#endregion

namespace Leoxia.Log
{
    /// <summary>
    ///     Factory of <see cref="ILogEvent" />
    /// </summary>
    /// <seealso cref="Leoxia.Log.ILogEventFactory" />
    public class LogEventFactory : ILogEventFactory
    {
        private static int _id;
        private readonly ITimestampProvider _provider;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LogEventFactory" /> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        public LogEventFactory(ITimestampProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        ///     Builds the <see cref="ILogEvent" /> with specified log level, topic and message.
        /// </summary>
        /// <param name="logLevel">The log level.</param>
        /// <param name="topic">The topic.</param>
        /// <param name="message">The message.</param>
        /// <returns>
        ///     <see cref="ILogEvent" />
        /// </returns>
        public ILogEvent Build(LogLevel logLevel, string topic, string message)
        {
            var logId = Interlocked.Increment(ref _id);
            var stamp = _provider.Now;
            var preciseTimestamp = _provider.PreciseTimestamp;
            var logEvent = new LogEvent(logId, logLevel, topic, message, stamp, preciseTimestamp,
                Thread.CurrentThread.ManagedThreadId.ToString(CultureInfo.InvariantCulture),
                Process.GetCurrentProcess().Id);
            return logEvent;
        }
    }
}