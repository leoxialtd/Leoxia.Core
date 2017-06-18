using System;
using System.Threading;
using System.Threading.Tasks;
using Leoxia.Abstractions;

namespace Leoxia.Threading
{    
    /// <summary>
    /// Utility class used to sleep until the specified delay period has elapsed
    /// </summary>
    public class Delayer
    {
        private readonly ITimeProvider _timeProvider;
        private readonly TimeSpan _delayPeriod;
        private DateTime _lastSleepCall;

        /// <summary>
        /// Initializes a new instance of the <see cref="Delayer"/> class.
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
        /// Puts this delayer to sleep.
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
