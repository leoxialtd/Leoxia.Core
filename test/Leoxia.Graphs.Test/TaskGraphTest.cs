#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskGraphTest.cs" company="Leoxia Ltd">
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

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Leoxia.Testing.Assertions;
using Xunit;

#endregion

namespace Leoxia.Graphs.Test
{
    public class TaskGraphTest
    {
        private readonly List<string> _leaves = new List<string>();
        private readonly List<string> _removeNames = new List<string>();
        private readonly object _synchro = new object();
        private readonly List<int> _values = new List<int>();
        private bool _error;

        [Fact]
        public void UseCase()
        {
            var set = new GraphSet<int>();
            var node = set.Add(3);
            var two = node.AddParent(2);
            two.AddParent(1);
            var task = set.ToTask(OnNode);
            task.Wait();
            Assert.Equal(1, _values[0]);
            Assert.Equal(2, _values[1]);
            Assert.Equal(3, _values[2]);
        }

        [Fact]
        public void ComplexCase()
        {
            var set = new GraphSet<string>();
            var three = set.Add("3");
            var four = three.AddChild("4");
            three.AddParent("2");
            var two = three.AddParent("Two");
            var one = two.AddParent("One");
            two.AddParent("Un");
            four.AddParent(one);
            var names = new List<string>();
            var task = set.ToTask(x => { OnNames(names, x); });
            task.Wait();
            lock (_synchro)
            {
                Assert.Equal(6, names.Count);
                Assert.True(names.IndexOf("3") < names.IndexOf("4"));
                Assert.True(names.IndexOf("Two") < names.IndexOf("3"));
                Assert.True(names.IndexOf("2") < names.IndexOf("3"));
                Assert.True(names.IndexOf("One") < names.IndexOf("Two"));
                Assert.True(names.IndexOf("Un") < names.IndexOf("Two"));
            }
        }

        [Fact]
        public void ComplexOtherTest()
        {
            var names = new List<string>();
            var set = new GraphSet<string>();
            var three = set.Add("3");
            three.AddChild("4");
            var two2 = three.AddParent("2");
            var two = three.AddParent("Two");
            two.AddChild("Three");
            two.AddParent("One");
            var un = two.AddParent("Un");
            un.AddChild(two2);

            var gtf = set.Add("greatfather");
            var gtm = set.Add("greatmother");
            var gf = gtf.AddChild("grandfather");
            gtm.AddChild(gf);
            var f = gf.AddChild("father");
            f.AddChild("me");
            var taskGraph = set.ToTaskGraph(x => { OnNames(names, x); });
            var tasks = taskGraph.GetNodes().Select(x => x.Value.Task).ToArray();
            Assert.Equal(12, tasks.Length);
            var task = Task.Factory.ContinueWhenAll(tasks, x => { });
            task.Wait();
            lock (_synchro)
            {
                Assert.False(_error, "Names contains " + names.FirstOrDefault());
                if (names.Count != 12)
                {
                    var message = "Names: " + string.Join(", ", names);
                    Assert.False(true, message);
                }
                Assert.True(names.IndexOf("3") < names.IndexOf("4"));
                Assert.True(names.IndexOf("Two") < names.IndexOf("3"));
                Assert.True(names.IndexOf("One") < names.IndexOf("Two"),
                    "One: " + names.IndexOf("One") + " Two: " + names.IndexOf("Two"));
                Assert.True(names.IndexOf("Un") < names.IndexOf("Two"));
                Check.That(names).Contains("One");
                Check.That(names).Contains("Two");
                Check.That(names).Contains("Three");
                Check.That(names).Contains("4");
                Check.That(names).Contains("Un");
                Check.That(names).Contains("father");
                Check.That(names).Contains("me");
                Check.That(names).Contains("grandfather");
                Check.That(names).Contains("greatfather");
                Check.That(names).Contains("greatmother");
            }
        }

        [Fact]
        public void FilterTest()
        {
            var set = new GraphSet<int>();
            var three = set.Add(3);
            var two = set.Add(2);
            var four = two.AddChild(4);
            var eight = two.AddChild(8);
            four.AddChild(eight);
            three.AddChild(9);
            two.AddChild(three.AddChild(6));
            var set2 = set.Filter(x => x % 2 == 0);
            Assert.False(set2.Any(x => x.Value == 3));
            Assert.False(set2.Any(x => x.Children.Any(c => c.Value == 3)));
            Assert.False(set2.Any(x => x.Parents.Any(p => p.Value == 3)));
            Assert.False(set2.Any(x => x.Value == 9));
            Assert.True(set2.All(x => x.Value % 2 == 0));
            Assert.Equal(4, set2.Count());
        }

        [Fact]
        public void ContinueWithTest()
        {
            var names = new List<string>();
            var set = new GraphSet<string>();
            var three = set.Add("3");
            three.AddChild("4");
            var two2 = three.AddParent("2");
            var two = three.AddParent("Two");
            two.AddParent("One");
            var un = two.AddParent("Un");
            un.AddChild(two2);
            var taskGraph = set.ToTaskGraph(x => { OnNames(names, x); });
            var taskCombined = taskGraph.ContinueWith(x => { OnRemoveNames(names, x); });
            var task = taskCombined.GetWhenAllTask();
            task.Wait();
            Assert.Equal(0, names.Count);
            Assert.False(_error);
        }

        [Fact]
        public void ContinueOnLeavesThenContinueWithTest()
        {
            var names = new List<string>();
            var set = new GraphSet<string>();
            var three = set.Add("3");
            three.AddChild("4");
            var two2 = three.AddParent("2");
            var two = three.AddParent("Two");
            two.AddChild("Three");
            two.AddParent("One");
            var un = two.AddParent("Un");
            un.AddChild(two2);

            var gtf = set.Add("greatfather");
            var gtm = set.Add("greatmother");
            var gf = gtf.AddChild("grandfather");
            gtm.AddChild(gf);
            var f = gf.AddChild("father");
            f.AddChild("me");
            var taskGraph = set.ToTaskGraph(x => { OnNames(names, x); });
            var taskLeaves = taskGraph.ContinueOnLeavesWith(OnLeaves);
            taskLeaves.GetWhenAllTask().Wait();
            var taskCombined = taskGraph.ContinueWith(x => { OnRemoveNames(names, x); });
            var task = taskCombined.GetWhenAllTask();
            task.Wait();
            //Thread.Sleep(1000);
            lock (_synchro)
            {
                Assert.False(_error, "Names contains " + names.FirstOrDefault());
                Assert.Equal(0, names.Count);
                Assert.True(_removeNames.IndexOf("3") < _removeNames.IndexOf("4"));
                Assert.True(_removeNames.IndexOf("Two") < _removeNames.IndexOf("3"));
                Assert.True(_removeNames.IndexOf("One") < _removeNames.IndexOf("Two"));
                Assert.True(_removeNames.IndexOf("Un") < _removeNames.IndexOf("Two"));
            }
        }

        private void OnLeaves(string obj)
        {
            lock (_synchro)
            {
                _leaves.Add(obj);
            }
        }

        private void OnRemoveNames(List<string> names, string data)
        {
            lock (_synchro)
            {
                if (!_error)
                {
                    if (data == null || names.Contains(data))
                    {
                        names.Remove(data);
                    }
                    else
                    {
                        _error = true;
                    }
                    _removeNames.Add(data);
                }
            }
        }

        private void OnNames(List<string> names, string data)
        {
            lock (_synchro)
            {
                if (data != null)
                {
                    names.Add(data);
                }
                else
                {
                    _error = true;
                }
            }
        }

        public void OnNode(int data)
        {
            _values.Add(data);
        }
    }

    public class MyData
    {
        public int Value { get; set; }
    }
}