#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILogEvent.cs" company="Leoxia Ltd">
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
    ///     Interface for logs
    /// </summary>
    public interface ILogEvent
    {
        /// <summary>
        ///     Gets the date.
        /// </summary>
        /// <value>
        ///     The date.
        /// </value>
        DateTime Date { get; }

        /// <summary>
        ///     Gets the identifier.
        /// </summary>
        /// <value>
        ///     The identifier.
        /// </value>
        int Id { get; }

        /// <summary>
        ///     Gets the level.
        /// </summary>
        /// <value>
        ///     The level.
        /// </value>
        LogLevel Level { get; }

        /// <summary>
        ///     Gets the message.
        /// </summary>
        /// <value>
        ///     The message.
        /// </value>
        string Message { get; }

        /// <summary>
        ///     Gets the precise timestamp.
        /// </summary>
        /// <value>
        ///     The precise timestamp.
        /// </value>
        long PreciseTimestamp { get; }

        /// <summary>
        ///     Gets the process identifier.
        /// </summary>
        /// <value>
        ///     The process identifier.
        /// </value>
        int ProcessId { get; }

        /// <summary>
        ///     Gets the thread identifier.
        /// </summary>
        /// <value>
        ///     The thread identifier.
        /// </value>
        string ThreadId { get; }

        /// <summary>
        ///     Gets the topic.
        /// </summary>
        /// <value>
        ///     The topic.
        /// </value>
        string Topic { get; }
    }
}