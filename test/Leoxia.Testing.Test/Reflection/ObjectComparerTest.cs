#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectComparerTest.cs" company="Leoxia Ltd">
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
using System.Collections.Generic;
using Leoxia.Testing.Reflection;
using Xunit;

#endregion

namespace Leoxia.Testing.Test.Reflection
{
    public class PropertyContainer : BaseContainer
    {
        public SubClassContainer SubClassContainer { get; set; }
    }

    public class BaseContainer
    {
        public ContainerEnum ContainerEnum { get; set; }
        public bool Flag { get; set; }
        public string Name { get; set; }
        public List<string> Names { get; set; }
        public int Number { get; set; }
        public double Price { get; set; }
        public TimeSpan Span { get; set; }
        public DateTime Timestamp { get; set; }
        public Type Type { get; set; }
    }

    public enum ContainerEnum
    {
        Default,
        Bar,
        Foo,
        Foobar
    }

    public class SubClassContainer : BaseContainer
    {
    }

    public class ObjectComparerTest
    {
        [Fact]
        public void Compare_Same_Instance_Should_Succeed()
        {
            var container = new PropertyContainer();
            Assert.True(ObjectFiller.Fill(container, true));
            Assert.True(Identity(container));
        }

        private static bool Identity<T>(T container)
        {
            return Compare(container, container);
        }

        private static bool Compare<TTested, TExpected>(TTested tested, TExpected expected)
        {
            return ObjectComparer.ObjectsAreEqual(tested, expected, PropertiesComparisonOptions.Default,
                new CheckingTrace());
        }

        [Fact]
        public void Compare_On_Basic_Instances()
        {
            Assert.False(Compare("foo", "bar"));
            Assert.True(Compare("foo", "foo"));
            Assert.False(Compare(1, 2));
            Assert.False(Compare(1.001, 1.000));
            Assert.False(Compare(1.001, "foo"));
            Assert.False(Compare((object) null, "foo"));
            Assert.True(Compare((object) null, (object) null));
            Assert.False(Compare("foo", (object) null));
            Assert.False(Compare((object) null, 1));
            Assert.False(Compare(DateTime.MaxValue, DateTime.Now));
            Assert.False(Compare(DateTime.MinValue, DateTime.Now));

            Assert.True(Identity(1));
            Assert.True(Identity("foo"));
            Assert.True(Identity(DateTime.Now));
            Assert.True(Identity(TimeSpan.FromMilliseconds(1)));
        }
    }
}