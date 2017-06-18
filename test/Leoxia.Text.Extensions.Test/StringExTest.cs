#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExTest.cs" company="Leoxia Ltd">
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

using Xunit;

#endregion

namespace Leoxia.Text.Extensions.Test
{
    public class StringExTest
    {
        [Fact]
        public void CheckCapitalizeResults()
        {
            Assert.Equal("Foo", "foo".Capitalize());
            Assert.Equal("Foo", "Foo".Capitalize());
            Assert.Equal("Bar", "bar".Capitalize());
            Assert.Equal("BAR", "BAR".Capitalize());
            Assert.Equal("BAR", "bAR".Capitalize());
        }

        [Fact]
        public void CheckUnCapitalizeResults()
        {
            Assert.Equal("foo", "foo".UnCapitalize());
            Assert.Equal("foo", "Foo".UnCapitalize());
            Assert.Equal("bar", "bar".UnCapitalize());
            Assert.Equal("bAR", "BAR".UnCapitalize());
            Assert.Equal("bAR", "bAR".UnCapitalize());
        }

        [Fact]
        public void CheckPlural()
        {
            Assert.Equal("Projects", "Project".Plural());
            Assert.Equal("Houses", "House".Plural());
            Assert.Equal("Technologies", "Technology".Plural());
            Assert.Equal("Images", "Image".Plural());
            Assert.Equal("Buses", "Bus".Plural());
            Assert.Equal("BUSES", "BUS".Plural());
        }
    }
}