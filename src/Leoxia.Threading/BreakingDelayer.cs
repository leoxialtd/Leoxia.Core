#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BreakingDelayer.cs" company="Leoxia Ltd">
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
using Leoxia.Abstractions;

#endregion

namespace Leoxia.Threading
{
    /// <summary>
    ///     Special type of delayer which is marked as broken after a specified number of sleep has been done.
    /// </summary>
    public class BreakingDelayer : Delayer
    {
        private readonly int _maxDelay;
        private volatile int _currentDelay;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Delayer" /> class.
        /// </summary>
        /// <param name="timeProvider">The time provider.</param>
        /// <param name="delayPeriod">The delay period.</param>
        /// <param name="maxDelay">The maximum delay after which delayer is broken.</param>
        public BreakingDelayer(ITimeProvider timeProvider, TimeSpan delayPeriod, int maxDelay) : base(timeProvider,
            delayPeriod)
        {
            _maxDelay = maxDelay;
        }

        /// <summary>
        ///     Gets a value indicating whether this instance is broken.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is broken; otherwise, <c>false</c>.
        /// </value>
        public bool IsBroken => _currentDelay == _maxDelay;

        /// <summary>
        ///     Put this delayer to sleep.
        /// </summary>
        public override void Sleep()
        {
            _currentDelay++;
            base.Sleep();
        }
    }
}