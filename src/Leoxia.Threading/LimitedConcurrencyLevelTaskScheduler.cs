#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LimitedConcurrencyLevelTaskScheduler.cs" company="Leoxia Ltd">
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
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace Leoxia.Threading
{
    // From https://msdn.microsoft.com/fr-fr/library/system.threading.tasks.taskscheduler%28v=vs.110%29.aspx
    // From https://msdn.microsoft.com/library/system.threading.tasks.taskscheduler.aspx
    // Note that we replaced ThreadPool.UnsafeQueueUserWorkItem by ThreadPool.QueueUserWorkItem which is maybe problematic
    /// <summary>
    ///     Provides a task scheduler that ensures a maximum concurrency level while
    ///     running on top of the thread pool.
    /// </summary>
    public class LimitedConcurrencyLevelTaskScheduler : TaskScheduler
    {
        // Indicates whether the current thread is processing work items.
        [ThreadStatic] private static bool _currentThreadIsProcessingItems;

        // The maximum concurrency level allowed by this scheduler. 
        private readonly int _maxDegreeOfParallelism;

        // The list of tasks to be executed 
        private readonly ConcurrentQueue<Task> _tasks = new ConcurrentQueue<Task>();

        // Indicates whether the scheduler is currently processing work items. 
        private int _delegatesQueuedOrRunning;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LimitedConcurrencyLevelTaskScheduler" /> class.
        /// </summary>
        /// <param name="maxDegreeOfParallelism">The maximum degree of parallelism.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">maxDegreeOfParallelism</exception>
        public LimitedConcurrencyLevelTaskScheduler(int maxDegreeOfParallelism)
        {
            if (maxDegreeOfParallelism < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(maxDegreeOfParallelism));
            }
            _maxDegreeOfParallelism = maxDegreeOfParallelism;
        }

        /// <summary>
        ///     Indicates the maximum concurrency level this <see cref="T:System.Threading.Tasks.TaskScheduler" /> is able to
        ///     support.
        /// </summary>
        public sealed override int MaximumConcurrencyLevel => _maxDegreeOfParallelism;


        /// <summary>
        ///     Queues a <see cref="T:System.Threading.Tasks.Task" /> to the scheduler.
        /// </summary>
        /// <param name="task">The <see cref="T:System.Threading.Tasks.Task" /> to be queued.</param>
        protected sealed override void QueueTask(Task task)
        {
            // Add the task to the list of tasks to be processed.  If there aren't enough 
            // delegates currently queued or running to process tasks, schedule another. 
            _tasks.Enqueue(task);
            if (Interlocked.CompareExchange(ref _delegatesQueuedOrRunning, _maxDegreeOfParallelism,
                    _maxDegreeOfParallelism) != _maxDegreeOfParallelism)
            {
                Interlocked.Increment(ref _delegatesQueuedOrRunning);
                NotifyThreadPoolOfPendingWork();
            }
        }


        // Inform the ThreadPool that there's work to be executed for this scheduler. 
        private void NotifyThreadPoolOfPendingWork()
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                // Note that the current thread is now processing work items.
                // This is necessary to enable inlining of tasks into this thread.
                _currentThreadIsProcessingItems = true;
                try
                {
                    // Process all available items in the queue.
                    while (true)
                    {
                        Task item;
                        // When there are no more items to be processed,
                        // note that we're done processing, and get out.
                        if (_tasks.Count == 0)
                        {
                            Interlocked.Decrement(ref _delegatesQueuedOrRunning);
                            break;
                        }
                        // Get the next item from the queue
                        if (_tasks.TryDequeue(out item))
                        {
                            TryExecuteTask(item);
                        }
                        else
                        {
                            Thread.Sleep(10);
                        }
                    }
                }
                // We're done processing items on the current thread
                finally
                {
                    _currentThreadIsProcessingItems = false;
                }
            });
        }

        /// <summary>
        ///     Determines whether the provided <see cref="T:System.Threading.Tasks.Task" /> can be executed synchronously in this
        ///     call, and if it can, executes it.
        /// </summary>
        /// <param name="task">The <see cref="T:System.Threading.Tasks.Task" /> to be executed.</param>
        /// <param name="taskWasPreviouslyQueued">
        ///     A Boolean denoting whether or not task has previously been queued. If this
        ///     parameter is True, then the task may have been previously queued (scheduled); if False, then the task is known not
        ///     to have been queued, and this call is being made in order to execute the task inline without queuing it.
        /// </param>
        /// <returns>
        ///     A Boolean value indicating whether the task was executed inline.
        /// </returns>
        protected sealed override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            // If this thread isn't already processing a task, we don't support inlining
            if (!_currentThreadIsProcessingItems)
            {
                return false;
            }

            // If the task was previously queued, remove it from the queue
            if (taskWasPreviouslyQueued)
                // Try to run the task. 
            {
                if (TryDequeue(task))
                {
                    return TryExecuteTask(task);
                }
                return false;
            }
            return TryExecuteTask(task);
        }

        /// <summary>
        ///     Attempts to dequeue a <see cref="T:System.Threading.Tasks.Task" /> that was previously queued to this scheduler.
        /// </summary>
        /// <param name="task">The <see cref="T:System.Threading.Tasks.Task" /> to be dequeued.</param>
        /// <returns>
        ///     A Boolean denoting whether the <paramref name="task" /> argument was successfully dequeued.
        /// </returns>
        // ReSharper disable once RedundantAssignment
        protected sealed override bool TryDequeue(Task task)
        {
            return _tasks.TryDequeue(out task);
        }


        /// <summary>
        ///     For debugger support only, generates an enumerable of <see cref="T:System.Threading.Tasks.Task" /> instances
        ///     currently queued to the scheduler waiting to be executed.
        /// </summary>
        /// <returns>
        ///     An enumerable that allows a debugger to traverse the tasks currently queued to this scheduler.
        /// </returns>
        protected sealed override IEnumerable<Task> GetScheduledTasks()
        {
            return _tasks;
        }
    }
}