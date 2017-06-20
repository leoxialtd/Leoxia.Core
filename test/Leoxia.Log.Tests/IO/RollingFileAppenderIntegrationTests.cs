#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RollingFileAppenderIntegrationTests.cs" company="Leoxia Ltd">
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

using System.IO;
using System.Linq;
using Leoxia.Log.IO;
using Xunit;

#endregion

namespace Leoxia.Log.Tests.IO
{
    public class RollingFileAppenderIntegrationTests
    {
        [Fact]
        public void IntegrationLogInSubDirectoryTest()
        {
            var directoryInfo = new DirectoryInfo(Directory.GetCurrentDirectory() + "/Logs");
            if (directoryInfo.Exists)
            {
                var files = directoryInfo.GetFiles("myTest*.Log");
                foreach (var file in files)
                {
                    file.Delete();
                }
            }
            var appender = new RollingFileAppender("Logs/myTest.Log");
            LogManager.AppenderMediator.Subscribe(appender);
            var logger = LogManager.GetLogger(typeof(RollingFileAppenderTests));
            logger.Info("Appender is logging");
            appender.Dispose();
            LogManager.AppenderMediator.Unsubscribe(appender);
            appender = new RollingFileAppender("Logs/myTest.Log");
            LogManager.AppenderMediator.Subscribe(appender);
            logger.Info("Appender is logging again");
            appender.Dispose();
            var logFiles = directoryInfo.GetFiles("myTest*.Log");
            Assert.Equal(2, logFiles.Length);
        }

        [Fact]
        public void IntegrationLogTest()
        {
            var files = new DirectoryInfo(Directory.GetCurrentDirectory()).GetFiles("myTest*.Log");
            foreach (var file in files)
            {
                file.Delete();
            }
            var appender = new RollingFileAppender("myTest.Log");
            LogManager.AppenderMediator.Subscribe(appender);
            var logger = LogManager.GetLogger(typeof(RollingFileAppenderTests));
            logger.Info("Appender is logging");
            appender.Dispose();
            LogManager.AppenderMediator.Unsubscribe(appender);
            appender = new RollingFileAppender("myTest.Log");
            LogManager.AppenderMediator.Subscribe(appender);
            logger.Info("Appender is logging again");
            appender.Dispose();
            files = new DirectoryInfo(Directory.GetCurrentDirectory()).GetFiles("myTest*.Log");
            Assert.Equal(2, files.Length);
        }

        [Fact]
        public void IntegrationLogWithRollForLengthTest()
        {
            var files = new DirectoryInfo(Directory.GetCurrentDirectory()).GetFiles("rollTest*.Log");
            foreach (var file in files)
            {
                file.Delete();
            }
            var appender = new RollingFileAppender("rollTest.Log");
            LogManager.AppenderMediator.Subscribe(appender);
            var logger = LogManager.GetLogger(typeof(RollingFileAppenderTests));
            logger.Info("Appender is logging");
            appender.Dispose();
            appender.MaxLength = 0;
            logger.Info("Appender is logging in a rolling file");
            appender.Dispose();
            files = new DirectoryInfo(Directory.GetCurrentDirectory()).GetFiles("rollTest*.Log");
            Assert.Equal(2, files.Length);

            LogManager.AppenderMediator.Unsubscribe(appender);
            appender = new RollingFileAppender("rollTest.Log");
            appender.MaxLength = 0;
            LogManager.AppenderMediator.Subscribe(appender);
            logger.Info("Appender is logging again");
            appender.Dispose();
            files = new DirectoryInfo(Directory.GetCurrentDirectory()).GetFiles("rollTest*.Log");
            Assert.Equal(3, files.Length);
        }

        [Fact]
        public void LogOnLockedFileTest()
        {
            var files = new DirectoryInfo(Directory.GetCurrentDirectory()).GetFiles("otherTest*.Log");
            foreach (var file in files)
            {
                file.Delete();
            }
            var appender = new RollingFileAppender("otherTest.Log");
            LogManager.AppenderMediator.Subscribe(appender);
            var logger = LogManager.GetLogger(typeof(RollingFileAppenderTests));
            logger.Info("Appender is logging");
            LogManager.AppenderMediator.Unsubscribe(appender);
            appender.Dispose();
            files = new DirectoryInfo(Directory.GetCurrentDirectory()).GetFiles("otherTest*.Log");
            Assert.Equal(1, files.Length);
            var fileLog = files.FirstOrDefault();
            // grab a lock on the log file
            using (File.Open(fileLog.FullName, FileMode.Open, FileAccess.Write, FileShare.None))
            {
                appender = new RollingFileAppender("otherTest.Log");
                LogManager.AppenderMediator.Subscribe(appender);
                logger.Info("Appender is logging again");
                appender.Dispose();
                files = new DirectoryInfo(Directory.GetCurrentDirectory()).GetFiles("otherTest*.Log");
                Assert.Equal(2, files.Length);
            }
        }
    }
}