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

namespace Leoxia.Testing.Mocks
{
    /// <summary>
    ///     A test unit fixture providing class IOC resolution and auto injection of mocks.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class MockUnitTestFixture : IDisposable
    {
        private readonly MockFactory _factory = new MockFactory();

        /// <summary>
        ///     Initializes a new instance of the <see cref="MockUnitTestFixture" /> class.
        /// </summary>
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
                return new DelegateFactory(x => _factory.Get(serviceType, Behavior).Object);
            }));
        }

        /// <summary>
        ///     Gets or sets the behavior used to create new <see cref="Mock" />.
        /// </summary>
        /// <value>
        ///     The behavior.
        /// </value>
        public MockBehavior Behavior { get; set; }

        /// <summary>
        ///     Gets the IOC container.
        /// </summary>
        /// <value>
        ///     The container.
        /// </value>
        public IContainer Container { get; }

        /// <summary>
        ///     Releases unmanaged resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Gets the mock for given interface.
        /// </summary>
        /// <typeparam name="T">type of mocked interface.</typeparam>
        /// <returns>
        ///     <see cref="Mock{T}" />
        /// </returns>
        public Mock<T> Get<T>() where T : class
        {
            return (Mock<T>) Get(typeof(T), Behavior, Lifetime.Singleton);
        }

        /// <summary>
        ///     Gets the mock the specified life time.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lifeTime">The life time.</param>
        /// <returns>
        ///     <see cref="Mock{T}" />
        /// </returns>
        public Mock<T> Get<T>(Lifetime lifeTime) where T : class
        {
            var mock = (Mock<T>) Get(typeof(T), Behavior, lifeTime);
            Register(mock);
            return mock;
        }

        private void Register<T>(Mock<T> mock) where T: class
        {
            Container.RegisterInstance<T>(mock.Object, ifAlreadyRegistered: IfAlreadyRegistered.Replace);
        }

        /// <summary>
        ///     Gets the mock with specified behavior.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="behavior">The behavior.</param>
        /// <returns>
        ///     <see cref="Mock{T}" />
        /// </returns>
        public Mock<T> Get<T>(MockBehavior behavior) where T : class
        {
            var mock = (Mock<T>) Get(typeof(T), behavior, Lifetime.Singleton);
            Register(mock);
            return mock;
        }

        /// <summary>
        ///     Gets the mock for the specified type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="behavior">The behavior.</param>
        /// <param name="lifetime">The lifetime.</param>
        /// <returns>
        ///     <see cref="Mock" />
        /// </returns>
        private Mock Get(Type type, MockBehavior behavior, Lifetime lifetime)
        {
            var mock = _factory.Get(type, behavior, lifetime);
            return mock;
        }

        /// <summary>
        ///     Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        ///     <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only
        ///     unmanaged resources.
        /// </param>
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