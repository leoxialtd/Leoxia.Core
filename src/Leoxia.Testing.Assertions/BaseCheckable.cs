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
    /// <summary>
    ///     Base class for all <see cref="ICheckable{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Leoxia.Testing.Assertions.Abstractions.ICheckable{T}" />
    public abstract class BaseCheckable<T> : ICheckable<T>
    {
        /// <summary>
        ///     The factory
        /// </summary>
        protected readonly IExceptionFactory _factory;

        /// <summary>
        ///     The value
        /// </summary>
        protected readonly T _value;


        /// <summary>
        ///     Initializes a new instance of the <see cref="BaseCheckable{T}" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="value">The value.</param>
        protected BaseCheckable(IExceptionFactory factory, T value)
        {
            _factory = factory;
            _value = value;
            // TODO: Capture Stack Trace in .NET Core 2.O
            // TODO: Retrieve the variable, property etc name to produce a better message.
        }

        /// <summary>
        ///     Gets the value.
        /// </summary>
        /// <value>
        ///     The value.
        /// </value>
        internal T Value => _value;

        /// <summary>
        ///     Determines whether [is] [the specified function].
        /// </summary>
        /// <param name="func">The function.</param>
        /// <returns></returns>
        public IBoolCheckable Is(Expression<Func<T, bool>> func)
        {
            return new ExpressionCheckable<T>(_factory, func, _value);
        }

        /// <summary>
        ///     Determines whether [is equal to] [the specified expected].
        /// </summary>
        /// <param name="expected">The expected.</param>
        /// <param name="message">The message.</param>
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

        /// <summary>
        ///     Determines whether [is not equal to] [the specified expected].
        /// </summary>
        /// <param name="expected">The expected.</param>
        /// <param name="message">The message.</param>
        public void IsNotEqualTo(T expected, string message = null)
        {
            if (_value.Equals(expected) && InnerIsNotEqualTo(expected, message))
            {
                Throw(expected, message, CheckType.NotEqual);
            }
        }

        /// <summary>
        ///     Determines whether [is operator equal to] [the specified expected].
        /// </summary>
        /// <param name="expected">The expected.</param>
        /// <param name="message">The message.</param>
        public void IsOperatorEqualTo(T expected, string message = null)
        {
            if (!_value.IsOperatorEqual(expected))
            {
                Throw(expected, message, CheckType.OpEqual);
            }
        }

        /// <summary>
        ///     Determines whether [is operator not equal to] [the specified expected].
        /// </summary>
        /// <param name="expected">The expected.</param>
        /// <param name="message">The message.</param>
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
            // ReSharper disable once UnthrowableException
            throw _factory.Build(equalCheckFailure);
        }

        /// <summary>
        ///     Checks the Inner is equal to
        /// </summary>
        /// <param name="expected">The expected.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        protected virtual bool InnerIsEqualTo(T expected, string message = null)
        {
            return false; // By default fail
        }

        /// <summary>
        ///     Checks the Inner is not equal to
        /// </summary>
        /// <param name="expected">The expected.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        protected virtual bool InnerIsNotEqualTo(T expected, string message = null)
        {
            return true; // By default succeed
        }
    }
}