#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeTest.cs" company="Leoxia Ltd">
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

namespace Leoxia.Reflection.Test
{
    public class CollectionContainer
    {
        public IList<string> Property { get; set; }
    }

    public class NoCollectionContainer
    {
        public string MyValue { get; set; }
    }

    public class TypeTest
    {
        [Fact]
        public void StringTypeTest()
        {
            var type = CachedType.Get(typeof(string));
            Assert.Equal("String", type.Name);
            Assert.False(type.HasCollectionProperties);
        }

        [Fact]
        public void GenericTypeTest()
        {
            var type = CachedType.Get(typeof(List<string>));
            Assert.Equal("List<String>", type.Name);
            Assert.False(type.HasCollectionProperties);
        }

        [Fact]
        public void ComplexGenericType()
        {
            var type = CachedType.Get(typeof(Dictionary<string, HashSet<List<string>>>));
            Assert.Equal("Dictionary<String, HashSet<List<String>>>", type.Name);
            Assert.True(type.HasCollectionProperties);
        }

        [Fact]
        public void CollectionTypeTest()
        {
            var type = CachedType.Get(typeof(CollectionContainer));
            Assert.Equal("CollectionContainer", type.Name);
            Assert.True(type.HasCollectionProperties);
            Assert.True(type.HasProperty("Property"));
            Assert.False(type.HasProperty("AnotherProperty"));
        }

        [Fact]
        public void NoCollectionTypeTest()
        {
            var type = CachedType.Get(typeof(NoCollectionContainer));
            Assert.Equal("NoCollectionContainer", type.Name);
            Assert.False(type.HasCollectionProperties);
            Assert.True(type.HasProperty("MyValue"));
            Assert.False(type.HasProperty("Property"));
        }
    }
}