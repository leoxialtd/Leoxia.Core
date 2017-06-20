#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ThreadTimer.cs" company="Leoxia Ltd">
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

#endregion

namespace Leoxia.Threading
{
    /// <summary>
    ///     Similar to System.Threading.Timer, it prevents from having concurrent callback calls.
    /// </summary>
    public sealed class ThreadTimer : IDisposable
    {
        private readonly TimerCallback _callback;
        private readonly TimeSpan _periodSpan;
        private readonly DateTime _startingDate;
        private readonly object _state;
        private readonly Thread _thread;

        private readonly int Precision = 100;
        private DateTime _nextCallDate;
        private volatile bool _running = true;
        private bool _started;


        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="callback">The callback.</param>
        /// <param name="state">The state to pass to callback.</param>
        /// <param name="dueTime">The due time in milliseconds before first call to callback.</param>
        /// <param name="period">The period in milliseconds between calls to callback.</param>
        public ThreadTimer(TimerCallback callback, object state, int dueTime, int period)
        {
            _callback = callback;
            _state = state;
            _thread = new Thread(OnLoop);
            CreationDate = DateTime.UtcNow;
            _startingDate = CreationDate + TimeSpan.FromMilliseconds(dueTime);
            _nextCallDate = _startingDate;
            _periodSpan = TimeSpan.FromMilliseconds(period);
            _thread.Start();
        }

        /// <summary>
        ///     Gets the creation date.
        /// </summary>
        /// <value>
        ///     The creation date.
        /// </value>
        public DateTime CreationDate { get; }


        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _running = false;
            if (!_thread.Join((int) TimeSpan.FromSeconds(1).TotalMilliseconds))
            {
                _thread.Abort();
                _thread.Join();
            }
        }

        private void OnLoop()
        {
            while (_running)
            {
                if (_started || WaitForStart())
                {
                    if (ShouldCallCallback())
                    {
                        _callback(_state);
                    }
                }
                Thread.Sleep(Precision);
            }
        }

        private bool ShouldCallCallback()
        {
            if (DateTime.UtcNow > _nextCallDate)
            {
                _nextCallDate = _nextCallDate + _periodSpan;
                return true;
            }
            return false;
        }

        private bool WaitForStart()
        {
            if (DateTime.UtcNow > _startingDate)
            {
                _started = true;
                return true;
            }
            return false;
        }
    }
}