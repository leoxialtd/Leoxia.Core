using System;
using System.Threading;

namespace Leoxia.Threading
{
    /// <summary>
    /// Mutex usable in a using clause.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public sealed class DisposableMutex : IDisposable
    {
        private readonly Mutex _locker;

        /// <summary>
        /// Initializes a new instance of the <see cref="DisposableMutex"/> class.
        /// </summary>
        /// <param name="locker">The locker.</param>
        public DisposableMutex(Mutex locker)
        {
            _locker = locker;
            _locker.WaitOne();
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            _locker.ReleaseMutex();
        }
    }
}