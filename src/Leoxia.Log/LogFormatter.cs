#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogFormatter.cs" company="Leoxia Ltd">
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

#endregion

namespace Leoxia.Log
{
    /// <summary>
    ///     Transform information fields of a log event to a formatted string.
    /// </summary>
    public class LogFormatter : ILogFormatter
    {
        /// <summary>
        ///     Formats the <see cref="ILogEvent" /> with the specified provider.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="logEvent">The log event.</param>
        /// <returns></returns>
        public string Format(ILogFormatProvider provider, ILogEvent logEvent)
        {
            var format = provider.GetFormat();
            var formattedDate = FormatDate(logEvent.Date, provider.GetDateFormat());
            return string.Format(format, formattedDate, logEvent.Topic, logEvent.Level, logEvent.Message,
                logEvent.ThreadId, logEvent.ProcessId, logEvent.Id, logEvent.PreciseTimestamp);
        }

        private string FormatDate(DateTime timestamp, string dateFormat)
        {
            return timestamp.ToString(dateFormat);
        }
    }
}