#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ILogger.cs" company="Leoxia Ltd">
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
    ///     Generic interface for all loggers.
    /// </summary>
    public interface ILogger
    {
        bool IsDebugEnabled { get; set; }

        /// <summary>
        ///     An Exception has occured and should be logged.
        /// </summary>
        /// <param name="message">message to log</param>
        /// <param name="exception">exception to log</param>
        void Exception(string message, Exception exception);

        /// <summary>
        ///     An error has occured and should be logged.
        /// </summary>
        /// <param name="message">message to log</param>
        void Error(string message);

        /// <summary>
        ///     Unknown has occurred and a warning should be logged.
        /// </summary>
        /// <param name="message">message to log</param>
        void Warn(string message);

        /// <summary>
        ///     An informative change has occured and should be notified.
        /// </summary>
        /// <param name="message">message to log</param>
        void Info(string message);

        /// <summary>
        ///     A detailed information on the internal behavior is available and could be logged.
        /// </summary>
        /// <param name="message">message to log</param>
        void Debug(string message);

        /// <summary>
        ///     An error has occured and should be logged.
        /// </summary>
        /// <param name="message">message format to log</param>
        /// <param name="parameters">parameters for formating</param>
        void ErrorFormat(string message, params object[] parameters);

        /// <summary>
        ///     Unknown has occurred and a warning should be logged.
        /// </summary>
        /// <param name="message">message to log</param>
        /// <param name="parameters">parameters for formating</param>
        void WarnFormat(string message, params object[] parameters);

        /// <summary>
        ///     An informative change has occured and should be notified.
        /// </summary>
        /// <param name="message">message format to log</param>
        /// <param name="parameters">parameters for formating</param>
        void InfoFormat(string message, params object[] parameters);

        /// <summary>
        ///     A detailed information on the internal behavior is available and could be logged.
        /// </summary>
        /// <param name="message">message format to log</param>
        /// <param name="parameters">parameters for formating</param>
        void DebugFormat(string message, params object[] parameters);
    }
}