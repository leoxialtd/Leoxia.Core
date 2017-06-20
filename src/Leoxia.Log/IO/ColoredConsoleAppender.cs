#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ColoredConsoleAppender.cs" company="Leoxia Ltd">
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
using Leoxia.Threading;

#endregion

namespace Leoxia.Log.IO
{
    /// <summary>
    ///     Appender of log event to Console standard output.
    /// </summary>
    public class ColoredConsoleAppender : IAppender
    {
        private readonly ISafeConsole _console;
        private readonly LogFormatter _formatter = new LogFormatter();
        private readonly ILogFormatProvider _provider;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ColoredConsoleAppender" /> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="console">The console.</param>
        public ColoredConsoleAppender(ILogFormatProvider provider = null, ISafeConsole console = null)
        {
            if (console == null)
            {
                console = SafeConsoleAdapter.Instance;
            }
            _console = console;
            if (provider == null)
            {
                provider = DefaultLogFormatProvider.Instance;
            }
            _provider = provider;
        }

        /// <summary>
        ///     Appends the specified log event.
        /// </summary>
        /// <param name="logEvent">The log event.</param>
        public void Append(ILogEvent logEvent)
        {
            var color = GetColor(logEvent);
            var message = _formatter.Format(_provider, logEvent);
            _console.SafeCall(c =>
            {
                var foreground = c.ForegroundColor;
                c.ForegroundColor = color;
                c.WriteLine(message);
                c.ForegroundColor = foreground;
            });
        }

        private ConsoleColor GetColor(ILogEvent logEvent)
        {
            switch (logEvent.Level)
            {
                case LogLevel.Error:
                    return ConsoleColor.Red;
                case LogLevel.Warn:
                    return ConsoleColor.Yellow;
                case LogLevel.Info:
                    return ConsoleColor.Cyan;
                case LogLevel.Debug:
                    return ConsoleColor.Green;
                default:
                    return ConsoleColor.White;
            }
        }
    }
}