#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RollingFileAppenderTest.cs" company="Leoxia Ltd">
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
using System.IO;
using System.Text;
using Leoxia.Abstractions;
using Leoxia.Abstractions.IO;
using Leoxia.Log.IO;
using Leoxia.Testing.Mock;
using Moq;
using Xunit;

#endregion

namespace Leoxia.Log.Unit.Tests
{
    public class RollingFileAppenderTest : MockUnitTestFixture
    {
        private const string Myfile = "myFile";
        private const string MyfileWithExtension = Myfile + ".Log";
        private static readonly string _currentdirectory = Path.Combine("Parent", "CurrentDirectory");
        private readonly RollingFileAppenderIntegrationTest _rollingFileAppenderIntegrationTest;
        private readonly ITimeProvider _timeProvider;
        private RollingFileAppender _appender;
        private string _basePath;
        private string _filePath;
        private string _filePathOne;
        private string _filePathTwo;
        private MemoryStream _stream;
        private DateTime _today = DateTime.MinValue;

        public RollingFileAppenderTest()
        {
            Behavior = MockBehavior.Strict;
            var timeProviderMock = Get<ITimeProvider>();
            timeProviderMock.SetupGet(x => x.Today).Returns(() => _today);
            _timeProvider = timeProviderMock.Object;
            SetupFileNames();
            _rollingFileAppenderIntegrationTest = new RollingFileAppenderIntegrationTest();
        }

        public void SetupFileNames()
        {
            _basePath = Path.Combine(_currentdirectory, Myfile + _today.ToString(".yyyy-MM-dd"));
            _filePath = _basePath + ".Log";
            _filePathOne = _basePath + "." + 1 + ".Log";
            _filePathTwo = _basePath + "." + 2 + ".Log";
        }

        [Fact]
        public void Append_On_Not_Existing_File_Test()
        {
            _stream = new MemoryStream();
            var fileInfoMock = Get<IFileInfo>();
            fileInfoMock.Setup(f => f.Refresh());
            fileInfoMock.SetupGet(f => f.Exists).Returns(false);
            fileInfoMock.SetupGet(f => f.FullName).Returns(_filePath);
            fileInfoMock.Setup(f => f.OpenWrite()).Returns(_stream);
            var fileInfo = fileInfoMock.Object;
            var factoryMock = Get<IFileInfoFactory>();
            factoryMock.Setup(x => x.Build(_filePath)).Returns(fileInfo);
            var factory = factoryMock.Object;
            var fileSystemMock = Get<IFile>();
            var fileSystem = fileSystemMock.Object;
            var directoryFileSystem = SetupDirectoryFileSystem();
            _appender = new RollingFileAppender(MyfileWithExtension,
                DefaultLogFormatProvider.Instance,
                factory,
                fileSystem,
                directoryFileSystem,
                _timeProvider);
            Append(_appender);
        }

        private void Append(RollingFileAppender appender)
        {
            var task = appender.AppendAsync(BuildLogEvent());
            task.Wait();
            CheckLog();
            _appender.Dispose();
        }

        private LogEvent BuildLogEvent()
        {
            return new LogEvent(0, LogLevel.Debug, "Topic", "Message", _today, 0, "1", 1);
        }

        [Fact]
        public void Append_On_Existing_File_With_No_Previous_File_Test()
        {
            var fileInfoMock = SetupFileInfo(true);
            var fileInfo = fileInfoMock.Object;
            var factoryMock = Get<IFileInfoFactory>();
            factoryMock.Setup(x => x.Build(_filePath)).Returns(fileInfo);
            var factory = factoryMock.Object;
            var fileSystemMock = Get<IFile>();
            fileSystemMock.Setup(x => x.Exists(_filePathOne)).Returns(false);
            fileSystemMock.Setup(x => x.Move(_filePath, _filePathOne));
            var fileSystem = fileSystemMock.Object;
            var directoryFileSystem = SetupDirectoryFileSystem();
            _appender = new RollingFileAppender(MyfileWithExtension,
                DefaultLogFormatProvider.Instance,
                factory,
                fileSystem,
                directoryFileSystem,
                _timeProvider);
            Append(_appender);
        }

        private IDirectory SetupDirectoryFileSystem()
        {
            var directoryFileSystemMock = Get<IDirectory>();
            directoryFileSystemMock.Setup(x => x.GetCurrentDirectory()).Returns(_currentdirectory);
            directoryFileSystemMock.Setup(x => x.Exists(It.IsAny<string>())).Returns(true);
            var directoryFileSystem = directoryFileSystemMock.Object;
            return directoryFileSystem;
        }

        [Fact]
        public void Append_On_Existing_File_With_One_Previous_File_Test()
        {
            var fileInfoMock = SetupFileInfo(true);
            var fileInfo = fileInfoMock.Object;
            var factoryMock = Get<IFileInfoFactory>();
            factoryMock.Setup(x => x.Build(_filePath)).Returns(fileInfo);
            var factory = factoryMock.Object;
            var fileSystemMock = Get<IFile>();
            fileSystemMock.Setup(x => x.Exists(_filePathOne)).Returns(true);
            fileSystemMock.Setup(x => x.Exists(_filePathTwo)).Returns(false);
            fileSystemMock.Setup(x => x.Move(_filePathOne, _filePathTwo));
            fileSystemMock.Setup(x => x.Move(_filePath, _filePathOne));
            var fileSystem = fileSystemMock.Object;
            var directoryFileSystem = SetupDirectoryFileSystem();
            _appender = new RollingFileAppender(MyfileWithExtension,
                DefaultLogFormatProvider.Instance,
                factory,
                fileSystem,
                directoryFileSystem,
                _timeProvider);
            Append(_appender);
        }


        [Fact]
        public void Append_With_Size_Over_Max_Size_And_No_Existing_File_Test()
        {
            var fileInfoMock = SetupFileInfo(true);
            var fileInfo = fileInfoMock.Object;
            var factoryMock = Get<IFileInfoFactory>();
            factoryMock.Setup(x => x.Build(_filePath)).Returns(fileInfo);
            var factory = factoryMock.Object;
            var fileSystemMock = Get<IFile>();
            fileSystemMock.Setup(x => x.Exists(_filePathOne)).Returns(false);
            fileSystemMock.Setup(x => x.Move(_filePath, _filePathOne));
            var fileSystem = fileSystemMock.Object;
            var directoryFileSystem = SetupDirectoryFileSystem();
            _appender = new RollingFileAppender(MyfileWithExtension,
                DefaultLogFormatProvider.Instance,
                factory,
                fileSystem,
                directoryFileSystem,
                _timeProvider);
            fileInfoMock.SetupGet(f => f.Length).Returns(_appender.MaxLength + 1);
            Append(_appender);
        }

        [Fact]
        public void Append_With_Changing_Of_Day_Test()
        {
            var fileInfoMock = SetupFileInfo(true);
            var fileInfo = fileInfoMock.Object;
            var factoryMock = Get<IFileInfoFactory>();
            factoryMock.Setup(x => x.Build(_filePath)).Returns(fileInfo);
            var factory = factoryMock.Object;
            var fileSystemMock = Get<IFile>();
            fileSystemMock.Setup(x => x.Exists(_filePathOne)).Returns(false);
            fileSystemMock.Setup(x => x.Move(_filePath, _filePathOne));
            var fileSystem = fileSystemMock.Object;
            var directoryFileSystem = SetupDirectoryFileSystem();
            _appender = new RollingFileAppender(MyfileWithExtension,
                DefaultLogFormatProvider.Instance,
                factory,
                fileSystem,
                directoryFileSystem,
                _timeProvider);
            fileInfoMock.SetupGet(f => f.Length).Returns(_appender.MaxLength + 1);
            var task = _appender.AppendAsync(BuildLogEvent());
            task.Wait();
            CheckLog();
            _today = DateTime.MinValue.AddDays(1);

            SetupFileNames();
            _stream = new MemoryStream();
            fileInfoMock = Get<IFileInfo>(Lifetime.Instance);
            fileInfoMock.Setup(f => f.Refresh());
            fileInfoMock.SetupGet(f => f.Exists).Returns(false);
            fileInfoMock.Setup(f => f.OpenWrite()).Returns(_stream);
            factoryMock.Setup(x => x.Build(_filePath)).Returns(fileInfoMock.Object);
            task = _appender.AppendAsync(BuildLogEvent());
            task.Wait();
            CheckLog();
        }

        private void CheckLog()
        {
            var log = Encoding.UTF8.GetString(_stream.ToArray());
            Assert.NotNull(log);
            Assert.True(log.Contains("Topic"));
            Assert.True(log.Contains("Message"));
            Assert.True(log.Contains(LogLevel.Debug.ToString()));
        }

        private Mock<IFileInfo> SetupFileInfo(bool exists)
        {
            _stream = new MemoryStream();
            var fileInfoMock = Get<IFileInfo>(Lifetime.Instance);
            fileInfoMock.Setup(f => f.Refresh());
            fileInfoMock.SetupGet(f => f.Exists).Returns(exists);
            fileInfoMock.SetupGet(f => f.Length).Returns(10);
            fileInfoMock.Setup(f => f.OpenWrite()).Returns(_stream);
            fileInfoMock.SetupGet(f => f.FullName).Returns(_filePath);
            return fileInfoMock;
        }
    }
}