#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseCheckable.cs" company="Leoxia Ltd">
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
using System.Linq.Expressions;
using Leoxia.Testing.Assertions.Abstractions;
using Leoxia.Testing.Assertions.Failures;
using Leoxia.Testing.Reflection;

#endregion

namespace Leoxia.Testing.Assertions
{
    public abstract class BaseCheckable<T> : ICheckable<T>
    {
        protected readonly IExceptionFactory _factory;
        protected readonly T _value;

        /// <summary>Initializes a new instance of the <see cref="T:System.Object" /> class.</summary>
        protected BaseCheckable(IExceptionFactory factory, T value)
        {
            _factory = factory;
            _value = value;
            // TODO: Capture Stack Trace in .NET Core 2.O
            // TODO: Retrieve the variable, property etc name to produce a better message.
        }

        internal T Value => _value;

        public IBoolCheckable Is(Expression<Func<T, bool>> func)
        {
            return new ExpressionCheckable<T>(_factory, func, _value);
        }

        public void IsEqualTo(T expected, string message = null)
        {
            if (ReferenceEquals(_value, expected))
            {
                return;
            }
            if (_value != null && _value.Equals(expected))
            {
                return;
            }
            if (!InnerIsEqualTo(expected, message))
            {
                Throw(expected, message, CheckType.Equal);
            }
        }

        public void IsNotEqualTo(T expected, string message = null)
        {
            if (_value.Equals(expected) && InnerIsNotEqualTo(expected, message))
            {
                Throw(expected, message, CheckType.NotEqual);
            }
        }

        public void IsOperatorEqualTo(T expected, string message = null)
        {
            if (!_value.IsOperatorEqual(expected))
            {
                Throw(expected, message, CheckType.OpEqual);
            }
        }

        public void IsOperatorNotEqualTo(T expected, string message = null)
        {
            if (_value.IsOperatorEqual(expected))
            {
                Throw(expected, message, CheckType.OpNotEqual);
            }
        }

        private void Throw(T expected, string message, CheckType checkType)
        {
            var equalCheckFailure = new EqualCheckFailure<T>(checkType, _value, expected, message);
            throw _factory.Build(equalCheckFailure);
        }

        protected virtual bool InnerIsEqualTo(T expected, string message = null)
        {
            return false; // By default fail
        }

        protected virtual bool InnerIsNotEqualTo(T expected, string message = null)
        {
            return true; // By default succeed
        }
    }

    public class ExpressionCheckable<T> : IBoolCheckable
    {
        private readonly Expression<Func<T, bool>> _expression;
        private readonly IExceptionFactory _factory;
        private readonly T _value;
        private readonly Func<T, bool> _func;

        public ExpressionCheckable(IExceptionFactory factory, Expression<Func<T, bool>> expression, T value)
        {
            _factory = factory;
            _expression = expression;
            _func = expression.Compile();
            _value = value;
        }

        public void IsTrue(string message = null)
        {
            if (!_func(_value))
            {
                throw _factory.Build(
                    new ExpressionCheckFailure<T>(CheckType.True, _value, default(T), message, _expression));
            }
        }

        public void IsFalse(string message = null)
        {
            if (_func(_value))
            {
                throw _factory.Build(new ExpressionCheckFailure<T>(CheckType.False, _value, default(T), message,
                    _expression));
            }
        }
    }

    public class ExpressionCheckFailure<T> : BaseCheckFailure<T>
    {
        private readonly Expression<Func<T, bool>> _expression;

        public ExpressionCheckFailure(CheckType type, T tested, T expected, string message,
            Expression<Func<T, bool>> expression) : base(type, tested, expected, message)
        {
            _expression = expression;
        }

        protected override string DisplayMessage()
        {
            switch (_type)
            {
                case CheckType.True:
                    return $"Check that {_tested} {_expression} is true: failure";
                case CheckType.False:
                    return $"Check that {_tested} {_expression} is false: failure";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}