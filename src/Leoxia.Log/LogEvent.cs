#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogEvent.cs" company="Leoxia Ltd">
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
    public class LogEvent : ILogEvent
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="LogEvent" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="level">The level.</param>
        /// <param name="topic">The topic.</param>
        /// <param name="message">The message.</param>
        /// <param name="date">The date.</param>
        /// <param name="timestamp">The timestamp.</param>
        /// <param name="threadId">The thread identifier.</param>
        /// <param name="processId">The process identifier.</param>
        public LogEvent(int id, LogLevel level, string topic, string message, DateTime date, long timestamp,
            string threadId, int processId)
        {
            ThreadId = threadId;
            Id = id;
            Level = level;
            Topic = topic;
            Message = message;
            PreciseTimestamp = timestamp;
            ProcessId = processId;
            Date = date;
        }

        public int Id { get; }
        public long PreciseTimestamp { get; }
        public int ProcessId { get; }

        /// <summary>
        ///     Gets the date.
        /// </summary>
        /// <value>
        ///     The date.
        /// </value>
        public DateTime Date { get; }

        /// <summary>
        ///     Gets the message.
        /// </summary>
        /// <value>
        ///     The message.
        /// </value>
        public string Message { get; }

        /// <summary>
        ///     Gets the topic.
        /// </summary>
        /// <value>
        ///     The topic.
        /// </value>
        public string Topic { get; }

        /// <summary>
        ///     Gets the level.
        /// </summary>
        /// <value>
        ///     The level.
        /// </value>
        public LogLevel Level { get; }

        /// <summary>
        ///     Gets or sets the thread identifier.
        /// </summary>
        /// <value>
        ///     The thread identifier.
        /// </value>
        public string ThreadId { get; }
    }
}