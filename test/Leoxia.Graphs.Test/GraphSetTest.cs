#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GraphSetTest.cs" company="Leoxia Ltd">
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

using System.Linq;
using Leoxia.Testing.Assertions;
using Xunit;

#endregion

namespace Leoxia.Graphs.Test
{
    public class GraphSetTest
    {
        [Fact]
        public void Test()
        {
            var set = new GraphSet<int>();
            var node = set.Add(3);
            Check.That(set.IsCyclic()).IsFalse();
            Check.That(node).IsNotNull();
            Check.That(node.Value).IsEqualTo(3);
            var nodes = set.GetNodes().ToList();
            Check.That(nodes.Count).IsEqualTo(1);
            Check.That(nodes.Select(x => x.Value).FirstOrDefault()).IsEqualTo(3);
        }

        [Fact]
        public void TestToGraphSet()
        {
            var set = new GraphSet<string>();
            var three = set.Add("3");
            var four = three.AddChild("4");
            three.AddParent("2");
            var two = three.AddParent("Two");
            var one = two.AddParent("One");
            two.AddParent("Un");
            four.AddParent(one);
            var nodes = set.GetNodes();
            Assert.Equal(6, nodes.Count());
            Assert.False(set.IsCyclic());
        }

        [Fact]
        public void TestIsCyclic()
        {
            var set = new GraphSet<string>();
            var three = set.Add("3");
            var four = three.AddParent("4");
            Assert.False(set.IsCyclic());
            three.AddChild(four);
            Assert.True(set.IsCyclic());
            four.AddParent("5");
            Assert.True(set.IsCyclic());
        }

        [Fact]
        public void TestComplexIsNotCyclic()
        {
            var set = new GraphSet<string>();
            var three = set.Add("3");
            var two = three.AddParent("2");
            var one = three.AddParent("1");
            Assert.False(set.IsCyclic());
            var root = two.AddParent("root");
            one.AddParent(root);
            Assert.False(set.IsCyclic());
        }
    }
}