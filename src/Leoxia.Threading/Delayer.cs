#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Delayer.cs" company="Leoxia Ltd">
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
using System.Threading;
using Leoxia.Abstractions;

#endregion

namespace Leoxia.Threading
{
    /// <summary>
    ///     Utility class used to sleep until the specified delay period has elapsed
    /// </summary>
    public class Delayer
    {
        private readonly TimeSpan _delayPeriod;
        private readonly ITimeProvider _timeProvider;
        private DateTime _lastSleepCall;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Delayer" /> class.
        /// </summary>
        /// <param name="timeProvider">The time provider.</param>
        /// <param name="delayPeriod">The delay period.</param>
        public Delayer(ITimeProvider timeProvider, TimeSpan delayPeriod)
        {
            _timeProvider = timeProvider;
            _delayPeriod = delayPeriod;
            _lastSleepCall = DateTime.MinValue;
        }

        /// <summary>
        ///     Puts this delayer to sleep.
        /// </summary>
        public virtual void Sleep()
        {
            var now = _timeProvider.Now;
            var notElapsed = _delayPeriod - (now - _lastSleepCall);
            if (notElapsed > TimeSpan.Zero)
            {
                Thread.Sleep(notElapsed);
            }
            _lastSleepCall = _timeProvider.Now;
        }
    }
}