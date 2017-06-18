#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogLevel.cs" company="Leoxia Ltd">
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

namespace Leoxia.Log
{
    /// <summary>
    ///     Log message criticity qualifier
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        ///     Abnormal behavior that cannot be fixed. Fatal for the application.
        ///     Must be traced in production.
        /// </summary>
        Fatal,

        /// <summary>
        ///     Abnormal behavior that should be avoided because nefarious for algorithm correct execution. Potentially fatal for
        ///     the application.
        ///     Must be traced in production.
        /// </summary>
        Error,

        /// <summary>
        ///     Abnormal behavior that could be an error but not for sure. Investigation should be done to refine the current case
        ///     in an information or a real error.
        ///     Must be traced in production.
        /// </summary>
        Warn,

        /// <summary>
        ///     A relevant information should be traced. Not useful by itself but needed in case of future errors.
        ///     Should be traced in production.
        /// </summary>
        Info,

        /// <summary>
        ///     Detailed information useful in order to trace a complex problem but potentially harmful to log readability,
        ///     application performance.
        ///     Should not be displayed in production.
        /// </summary>
        Debug
    }
}