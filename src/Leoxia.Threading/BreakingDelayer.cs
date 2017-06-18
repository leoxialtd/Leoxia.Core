using System;
using Leoxia.Abstractions;

namespace Leoxia.Threading
{
    /// <summary>
    /// Special type of delayer which is marked as broken after a specified number of sleep has been done. 
    /// </summary>
    public class BreakingDelayer : Delayer
    {
        private readonly int _maxDelay;
        private volatile int _currentDelay = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="Delayer" /> class.
        /// </summary>
        /// <param name="timeProvider">The time provider.</param>
        /// <param name="delayPeriod">The delay period.</param>
        /// <param name="maxDelay">The maximum delay after which delayer is broken.</param>
        public BreakingDelayer(ITimeProvider timeProvider, TimeSpan delayPeriod, int maxDelay) : base(timeProvider, delayPeriod)
        {
            _maxDelay = maxDelay;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is broken.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is broken; otherwise, <c>false</c>.
        /// </value>
        public bool IsBroken
        {
            get { return _currentDelay == _maxDelay; }
        }

        /// <summary>
        /// Put this delayer to sleep.
        /// </summary>
        public override void Sleep()
        {
            _currentDelay++;
            base.Sleep();
        }
    }
}