#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogManager.cs" company="Leoxia Ltd">
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
using Leoxia.Implementations;

#endregion

namespace Leoxia.Log
{
    /// <summary>
    ///     Factory responsible for building one Logger for a given topic.
    /// </summary>
    public static class LogManager
    {
        private static readonly ILogEventFactory _factory = new LogEventFactory(new TimestampProvider(new TimeProvider()));

        /// <summary>
        ///     Gets the log event manager.
        /// </summary>
        /// <value>
        ///     The log event manager.
        /// </value>
        public static IAppenderMediator AppenderMediator { get; } = new AppenderMediator();

        /// <summary>
        ///     Gets the logger with the given topic.
        /// </summary>
        /// <param name="topic">The topic.</param>
        /// <returns></returns>
        public static ILogger GetLogger(string topic)
        {
            return new Logger(AppenderMediator, _factory, topic);
        }

        /// <summary>
        ///     Gets the logger using type name as a topic.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static ILogger GetLogger(Type type)
        {
            return new Logger(AppenderMediator, _factory, type.Name);
        }
    }
}