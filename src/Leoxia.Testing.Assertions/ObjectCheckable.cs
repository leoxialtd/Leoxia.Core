#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectCheckable.cs" company="Leoxia Ltd">
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
using Leoxia.Testing.Assertions.Abstractions;
using Leoxia.Testing.Assertions.Failures;
using Leoxia.Testing.Reflection;

#endregion

namespace Leoxia.Testing.Assertions
{
    public class ObjectCheckable<T> : BaseClassCheckable<T>
        where T : class
    {
        public ObjectCheckable(IExceptionFactory factory, T value) : base(factory, value)
        {
        }

        protected override bool InnerIsEqualTo(T expected, string message = null)
        {
            var trace = new CheckingTrace();
            if (!ObjectComparer.ObjectsAreEqual(_value, expected, PropertiesComparisonOptions.Default, trace))
            {
                throw _factory.Build(new ObjectCheckFailure(CheckType.Equal, _value, expected, trace, message));
            }
            return true;
        }

        protected override bool InnerIsNotEqualTo(T expected, string message = null)
        {
            var trace = new CheckingTrace();
            if (ObjectComparer.ObjectsAreEqual(_value, expected, PropertiesComparisonOptions.Default, trace))
            {
                throw _factory.Build(new ObjectCheckFailure(CheckType.NotEqual, _value, expected, trace, message));
            }
            return true;
        }
    }

    public class ObjectCheckFailure : BaseCheckFailure<object>
    {
        private readonly CheckingTrace _trace;

        public ObjectCheckFailure(CheckType type, object tested, object expected, CheckingTrace trace,
            string message) : base(type, tested, expected, message)
        {
            _trace = trace;
        }

        protected override string DisplayMessage()
        {
            switch (_type)
            {
                case CheckType.Equal:
                {
                    return $"Check that {_tested} is equal to {_expected}: failure" + Environment.NewLine +
                           _trace;
                }
                case CheckType.NotEqual:
                {
                    return $"Check that {_tested} is not equal to {_expected}: failure" + Environment.NewLine +
                           _trace;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}