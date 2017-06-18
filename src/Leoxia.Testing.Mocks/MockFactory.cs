#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MockFactory.cs" company="Leoxia Ltd">
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
using System.Linq;
using Moq;

#endregion

namespace Leoxia.Testing.Mock
{
    public class MockFactory
    {
        private readonly List<Moq.Mock> _instanceMocks = new List<Moq.Mock>();
        private readonly IDictionary<Type, Moq.Mock> _mocks = new Dictionary<Type, Moq.Mock>();

        public IList<Moq.Mock> Mocks
        {
            get
            {
                var res = _instanceMocks.ToList();
                res.AddRange(_mocks.Values);
                return res.ToArray();
            }
        }

        public Moq.Mock Get(Type type, MockBehavior behavior = MockBehavior.Default,
            Lifetime lifetime = Lifetime.Singleton)
        {
            Moq.Mock instance;
            if (lifetime == Lifetime.Singleton)
            {
                if (!_mocks.TryGetValue(type, out instance))
                {
                    instance = CreateMock(type, behavior);
                    _mocks.Add(type, instance);
                }
            }
            else
            {
                instance = CreateMock(type, behavior);
                _instanceMocks.Add(instance);
            }
            return instance;
        }

        private static Moq.Mock CreateMock(Type type, MockBehavior behavior)
        {
            var mockType = typeof(MockProxy<>).MakeGenericType(type);
            var proxy = (IMockProxy) Activator.CreateInstance(mockType);
            return proxy.CreateMock(behavior);
        }
    }
}