#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RollingFileAppender.cs" company="Leoxia Ltd">
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
using System.IO;
using System.Threading.Tasks;
using Leoxia.Abstractions;
using Leoxia.Abstractions.IO;
using Leoxia.Implementations;
using Leoxia.Implementations.IO;
using Leoxia.Threading;

#endregion

namespace Leoxia.Log.IO
{
    /// <summary>
    ///     File Appender rolling the file depending on date.
    /// </summary>
    /// <seealso cref="Leoxia.Log.IAppender" />
    /// <seealso cref="System.IDisposable" />
    public sealed class RollingFileAppender : IAppender, IDisposable
    {
        private readonly string _baseFile;
        private readonly ConcurrentQueue<ILogEvent> _events = new ConcurrentQueue<ILogEvent>();
        private readonly IFileInfoFactory _fileFactory;
        private readonly IFile _fileSystem;
        private readonly LogFormatter _logFormatter = new LogFormatter();
        private readonly ILogFormatProvider _provider;
        private readonly TaskFactory _taskFactory;
        private readonly ConcurrentQueue<Task> _tasks = new ConcurrentQueue<Task>();
        private readonly ITimeProvider _timeProvider;
        private IFileInfo _file;
        private bool _locked;
        private StreamWriter _writer;

        /// <summary>
        ///     Initializes a new instance of the <see cref="RollingFileAppender" /> class.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="provider">The provider.</param>
        public RollingFileAppender(string file,
            ILogFormatProvider provider = null) :
            this(file, provider, new FileInfoFactory(), new FileAdapter(),
                new DirectoryAdapter(), new TimeProvider())
        {
        }


        /// <summary>
        ///     Initializes a new instance of the <see cref="RollingFileAppender" /> class.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="provider">The provider.</param>
        /// <param name="fileFactory">The file factory.</param>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="directoryFileSystem">The directory file system.</param>
        /// <param name="timeProvider">The time provider.</param>
        public RollingFileAppender(
            string file,
            ILogFormatProvider provider,
            IFileInfoFactory fileFactory,
            IFile fileSystem,
            IDirectory directoryFileSystem,
            ITimeProvider timeProvider)
        {
            if (!Path.IsPathRooted(file))
            {
                // HACK: We do that to avoid streamwriterfactory to create file 
                // where the System.IO nuget package cache location... 
                // Depends on the test runner
                file = Path.Combine(directoryFileSystem.GetCurrentDirectory(), file);
            }
            var directory = Path.GetDirectoryName(file);
            if (!directoryFileSystem.Exists(directory))
            {
                directoryFileSystem.CreateDirectory(directory);
            }
            if (provider == null)
            {
                provider = DefaultLogFormatProvider.Instance;
            }
            var taskScheduler = new LimitedConcurrencyLevelTaskScheduler(1);
            _taskFactory = new TaskFactory(taskScheduler);
            _provider = provider;
            _fileSystem = fileSystem;
            _timeProvider = timeProvider;
            _baseFile = file;
            _fileFactory = fileFactory;
            BuildFile();
            if (_file.Exists)
            {
                // Always roll on start logging
                RollFiles();
            }
            MaxLength = 1024 * 1024;
        }

        /// <summary>
        ///     Gets or sets the maximum length.
        /// </summary>
        /// <value>
        ///     The maximum length.
        /// </value>
        public long MaxLength { get; set; }

        /// <summary>
        ///     Appends the specified log event.
        /// </summary>
        /// <param name="logEvent">The log event.</param>
        public void Append(ILogEvent logEvent)
        {
            AppendAsync(logEvent);
        }


        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            foreach (var task in _tasks)
            {
                task.Wait();
            }
            CleanWriter();
        }

        private bool BuildFile()
        {
            var file = BuildFileName(0);
            if (_file == null || _file.FullName != file)
            {
                _file = _fileFactory.Build(file);
                return true;
            }
            return false;
        }

        private string BuildFileName(int counter)
        {
            var dateComponent = _timeProvider.Today.ToString(".yyyy-MM-dd");
            var filePart = Path.GetFileNameWithoutExtension(_baseFile);
            var directoryPart = Path.GetDirectoryName(_baseFile);
            var fileExtension = Path.GetExtension(_baseFile);
            var counterPart = string.Empty;
            if (counter != 0)
            {
                counterPart = "." + counter;
            }
            var lockedPart = string.Empty;
            if (_locked)
            {
                // File is locked. We add a marker in the file name to mitigate issue.
                lockedPart = "-locked";
            }
            return Path.Combine(directoryPart, filePart + dateComponent + lockedPart + counterPart + fileExtension);
        }

        /// <summary>
        ///     Appends the <see cref="ILogEvent" /> asynchronously.
        /// </summary>
        /// <param name="logEvent">The log event.</param>
        /// <returns></returns>
        public Task AppendAsync(ILogEvent logEvent)
        {
            _events.Enqueue(logEvent);
            var task = _taskFactory.StartNew(OnAppend);
            _tasks.Enqueue(task);
            return task;
        }

        private void OnAppend()
        {
            ILogEvent logEvent;
            while (_events.TryDequeue(out logEvent))
            {
                SecureAppend(logEvent);
            }
            Task task;
            var list = new List<Task>();
            while (_tasks.TryDequeue(out task))
            {
                if (!task.IsCompleted)
                {
                    list.Add(task);
                }
            }
            foreach (var item in list)
            {
                _tasks.Enqueue(item);
            }
        }

        private void SecureAppend(ILogEvent logEvent)
        {
            if (BuildFile())
            {
                CleanWriter();
            }
            // TODO: Remove this file access to improve performance
            // TODO: Count bytes written in memory instead.
            // TODO: Maybe use a wrapper class above writer and file
            _file.Refresh();
            if (_file.Exists && _file.Length > MaxLength)
            {
                CleanWriter();
                RollFiles();
            }
            if (_writer == null)
            {
                try
                {
                    BuildWriter();
                }
                catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
                {
                    _locked = true;
                    BuildFile();
                    BuildWriter();
                }
            }
            _writer.WriteLine(_logFormatter.Format(_provider, logEvent));
        }

        private void BuildWriter()
        {
            _writer = new StreamWriter(_file.OpenWrite());
            // TODO: Remove this to improve performance
            _writer.AutoFlush = true;
            _file.Refresh();
        }

        private void CleanWriter()
        {
            if (_writer != null)
            {
                _writer.Flush();
                _writer.Dispose();
                _writer = null;
            }
        }

        private void RollFiles()
        {
            var counter = 1;
            while (_fileSystem.Exists(GetRolledFileName(counter)))
            {
                counter++;
            }
            for (var i = counter - 1; i > 0; i--)
            {
                Move(i);
            }
            if (_file.Exists)
            {
                try
                {
                    _fileSystem.Move(_file.FullName, GetRolledFileName(1));
                    _file.Refresh();
                }
                catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
                {
                    _locked = true;
                    BuildFile();
                }
            }
        }

        private void Move(int index)
        {
            _fileSystem.Move(GetRolledFileName(index), GetRolledFileName(index + 1));
        }

        private string GetRolledFileName(int index)
        {
            return BuildFileName(index);
        }
    }
}