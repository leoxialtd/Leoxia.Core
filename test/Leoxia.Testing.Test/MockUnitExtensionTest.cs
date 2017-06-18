#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockUnitExtensionTest.cs" company="Leoxia Ltd">
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
using System.Collections;
using DryIoc;
using Leoxia.Testing.Mock;
using Moq;
using Xunit;

#endregion

namespace Leoxia.Testing.Test
{
    public class MockUnitExtensionTest : IDisposable
    {
        private readonly MockUnitTestFixture _fixture;

        public MockUnitExtensionTest()
        {
            _fixture = new MockUnitTestFixture();
        }

        public void Dispose()
        {
            _fixture?.Dispose();
        }

        [Fact]
        public void AddExtensionTest()
        {
            var list = _fixture.Container.Resolve<IList>();
            Assert.NotNull(list);
            var mock = _fixture.Get<IList>();
            Assert.StrictEqual(mock.Object, list);
        }

        [Fact]
        public void SetupTest()
        {
            var list = _fixture.Container.Resolve<IList>();
            var mock = _fixture.Get<IList>();
            mock.SetupGet(x => x.Count).Returns(1);
            Assert.StrictEqual(1, list.Count);
        }

        [Fact]
        public void StrictAndTeardownSucceedTest()
        {
            _fixture.Behavior = MockBehavior.Strict;
            var list = _fixture.Container.Resolve<IList>();
            var mock = _fixture.Get<IList>();
            mock.Setup(x => x.Contains(It.IsAny<object>())).Returns(true);
            Assert.True(list.Contains("Foobar"));
        }

        [Fact]
        public void OverrideTest()
        {
            var arrayList = new ArrayList();
            _fixture.Container.UseInstance<IList>(arrayList);
            var list = _fixture.Container.Resolve<IList>();
            Assert.StrictEqual(arrayList, list);
        }
    }
}