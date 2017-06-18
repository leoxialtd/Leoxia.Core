#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphComparerTest.cs" company="Leoxia Ltd">
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
using Xunit;

#endregion

namespace Leoxia.Graphs.Test
{
    public class GraphComparerTest
    {
        [Fact]
        public void CompareTest()
        {
            var set = new GraphSet<int>();
            var second = set.Add(2);
            var first = set.Add(1);
            second.AddParent(first);
            var third = set.Add(3);
            third.AddParent(second);
            var comparer = new GraphNodeComparer<int>();
            Assert.Equal(-1, comparer.Compare(second, third));
            Assert.Equal(1, comparer.Compare(third, first));
        }

        [Fact]
        public void SortListTest()
        {
            var set = new GraphSet<int>();
            var list = new List<GraphNode<int>>();
            var last = set.Add(100);
            list.Add(last);
            for (var i = 99; i > -1; i--)
            {
                var newNode = set.Add(i);
                list.Add(newNode);
                last.AddParent(newNode);
                last = newNode;
            }
            list.Reverse(20, 30);
            list.Reverse(40, 30);
            var comparer = new GraphNodeComparer<int>();
            list.Sort(comparer);
            for (var i = 0; i <= 100; i++)
            {
                Assert.Equal(i, list[i].Value);
            }
        }

        [Fact]
        public void SortMergedListTest()
        {
            var firstList = BuildList(0, 100);
            firstList.Reverse(20, 30);
            firstList.Reverse(40, 30);
            var comparer = new GraphNodeComparer<string>();
            firstList.Sort(comparer);
            for (var i = 0; i <= 100; i++)
            {
                Assert.Equal(i, int.Parse(firstList[i].Value));
            }
        }

        private static List<GraphNode<string>> BuildList(int start, int end)
        {
            var list = new List<GraphNode<string>>();
            var set = new GraphSet<string>();
            var last = set.Add(end.ToString());
            list.Add(last);
            for (var i = end - 1; i > start - 1; i--)
            {
                var newNode = set.Add(i.ToString());
                list.Add(newNode);
                last.AddParent(newNode);
                last = newNode;
            }
            return list;
        }
    }
}