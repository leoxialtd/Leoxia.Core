#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockUnitTestFixture.cs" company="Leoxia Ltd">
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
using System.Reflection;
using DryIoc;
using Moq;

#endregion

namespace Leoxia.Testing.Mock
{
    public class MockUnitTestFixture : IDisposable
    {
        private readonly MockFactory _factory = new MockFactory();

        public MockUnitTestFixture()
        {
            Behavior = MockBehavior.Default;
            Container = new Container(rules => rules.WithUnknownServiceResolvers(request =>
            {
                var serviceType = request.ServiceType;
                if (!serviceType.GetTypeInfo().IsAbstract)
                {
                    return null; // Mock interface or abstract class only.
                }
                return new DelegateFactory(x => _factory.Get(serviceType, Behavior, Lifetime.Singleton).Object);
            }));
        }

        public MockBehavior Behavior { get; set; }

        public IContainer Container { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public Mock<T> Get<T>() where T : class
        {
            return (Mock<T>) Get(typeof(T), Behavior, Lifetime.Singleton);
        }

        public Mock<T> Get<T>(Lifetime lifeTime) where T : class
        {
            return (Mock<T>) Get(typeof(T), Behavior, lifeTime);
        }

        public Mock<T> Get<T>(MockBehavior behavior) where T : class
        {
            return (Mock<T>) Get(typeof(T), behavior, Lifetime.Singleton);
        }

        private Moq.Mock Get(Type type, MockBehavior behavior, Lifetime lifetime)
        {
            return _factory.Get(type, behavior, lifetime);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var mock in _factory.Mocks)
                {
                    mock.VerifyAll();
                }
            }
        }
    }
}