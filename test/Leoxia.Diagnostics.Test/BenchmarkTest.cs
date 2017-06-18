using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Leoxia.Diagnostics;
using Moq;
using Xunit;

namespace Leoxia.Diagnostics.Test
{
    public class BenchmarkTest
    {
        private static readonly List<Task> _tasks = new List<Task>();

        [Fact]
        public void UseCase()
        {
            var watchMock = new Mock<IStopwatch>();
            watchMock.Setup(x => x.Elapsed).Returns(TimeSpan.FromMilliseconds(100));
            var watch = watchMock.Object;
            var factoryMock = new Mock<IStopwatchFactory>();            
            factoryMock.Setup(x => x.Build()).Returns(watch);
            var factory = factoryMock.Object;
            ProfilingManager manager = new ProfilingManager(factory);
            var categoryRecorder = manager.GetCategoryRecorder("MyCategory");
            categoryRecorder.Start();
            categoryRecorder.Stop();            
            var str = manager.GetDetailedSummary().ToString();
            Assert.Equal("MyCategory : 00:00:00.1000000" + Environment.NewLine, str);
        }

        [Fact]
        public void SeveralCategoryUseCase()
        {
            var factoryMock = new Mock<IStopwatchFactory>();
            factoryMock.Setup(x => x.Build()).Returns(SetupWatch(100));
            var factory = factoryMock.Object;
            ProfilingManager manager = new ProfilingManager(factory);
            using (manager.GetRecorder("MyCategory")){}
            factoryMock.Setup(x => x.Build()).Returns(SetupWatch(200));
            using (manager.GetRecorder("AnotherCategory")){}
            using (manager.GetRecorder("MyCategory")) { }
            factoryMock.Setup(x => x.Build()).Returns(SetupWatch(300));
            using (manager.GetRecorder("AnotherCategory")) { }
            var str = manager.GetDetailedSummary().ToString();
            Assert.Equal(
                "AnotherCategory : 00:00:00.5000000" + Environment.NewLine +
                "MyCategory : 00:00:00.3000000" + Environment.NewLine, 
                str);
        }

        [Fact]
        public void ThreadedSeveralCategoryUseCase()
        {
            var factoryMock = new Mock<IStopwatchFactory>();
            factoryMock.Setup(x => x.Build()).Returns(SetupWatch(100));
            var factory = factoryMock.Object;
            ProfilingManager manager = new ProfilingManager(factory);
            Theaded(manager, "AnotherCategory");
            Theaded(manager, "MyCategory");
            Theaded(manager, "AnotherCategory");
            Theaded(manager, "MyCategory");
            Theaded(manager, "AnotherCategory");
            Theaded(manager, "MyCategory");
            Theaded(manager, "AnotherCategory");
            Task.WaitAll(_tasks.ToArray());
            var str = manager.GetDetailedSummary().ToString();
            Assert.Equal(
                "AnotherCategory : 00:00:00.4000000" + Environment.NewLine +
                "MyCategory : 00:00:00.3000000" + Environment.NewLine,
                str);
        }

        [Fact]
        public void IntegrationThreadedSeveralCategoryUseCase()
        {
            ProfilingManager manager = new ProfilingManager();
            TheadedIntegrated(manager, "AnotherCategory");
            TheadedIntegrated(manager, "MyCategory");
            TheadedIntegrated(manager, "AnotherCategory");
            TheadedIntegrated(manager, "MyCategory");
            TheadedIntegrated(manager, "AnotherCategory");
            TheadedIntegrated(manager, "MyCategory");
            TheadedIntegrated(manager, "AnotherCategory");
            Task.WaitAll(_tasks.ToArray());
            var str = manager.GetDetailedSummary().ToString();
            //Assert.Equal(
            //    "AnotherCategory : 00:00:00.4000000" + Environment.NewLine +
            //    "MyCategory : 00:00:00.3000000" + Environment.NewLine,
            //    str);
        }


        private static void Theaded(ProfilingManager manager, string anothercategory)
        {
            _tasks.Add(Task.Factory.StartNew(() =>
            {
                using (manager.GetRecorder(anothercategory))
                {
                }
            }));
        }

        private static void TheadedIntegrated(ProfilingManager manager, string anothercategory)
        {
            _tasks.Add(Task.Factory.StartNew(() =>
            {
                using (manager.GetRecorder(anothercategory))
                {
                    Thread.Sleep(100);
                }
            }));
        }


        private static IStopwatch SetupWatch(int value)
        {
            var watchMock = new Mock<IStopwatch>();
            watchMock.Setup(x => x.Elapsed).Returns(TimeSpan.FromMilliseconds(value));
            var watch = watchMock.Object;
            return watch;
        }
    }
}
