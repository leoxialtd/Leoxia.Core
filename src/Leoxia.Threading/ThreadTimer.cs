using System;
using System.Threading;

namespace Leoxia.Threading
{    
    /// <summary>
    /// Similar to System.Threading.Timer, it prevents from having concurrent callback calls.
    /// </summary>
    public sealed class ThreadTimer : IDisposable
    {
        private readonly TimerCallback _callback;
        private readonly object _state;
        private volatile bool _running = true;
        private readonly Thread _thread;
        private readonly DateTime _startingDate;
        private DateTime _nextCallDate;
        private readonly TimeSpan _periodSpan;
        private bool _started;

        private int Precision = 100;


        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
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

        public DateTime CreationDate { get; }

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


        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _running = false;
            if (!_thread.Join((int)TimeSpan.FromSeconds(1).TotalMilliseconds))
            {
                _thread.Abort();
                _thread.Join();
            }
        }
    }
}
