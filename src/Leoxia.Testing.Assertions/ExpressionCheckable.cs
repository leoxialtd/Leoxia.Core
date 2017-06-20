#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExpressionCheckable.cs" company="Leoxia Ltd">
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

#endregion

namespace Leoxia.Testing.Assertions
{
    /// <summary>
    ///     Checks for <see cref="Predicate{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Leoxia.Testing.Assertions.Abstractions.IBoolCheckable" />
    public class ExpressionCheckable<T> : IBoolCheckable
    {
        private readonly Expression<Func<T, bool>> _expression;
        private readonly IExceptionFactory _factory;
        private readonly Func<T, bool> _func;
        private readonly T _value;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExpressionCheckable{T}" /> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="value">The value.</param>
        public ExpressionCheckable(IExceptionFactory factory, Expression<Func<T, bool>> expression, T value)
        {
            _factory = factory;
            _expression = expression;
            _func = expression.Compile();
            _value = value;
        }

        /// <summary>
        ///     Checks the current value is true.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="ExpressionCheckFailure{T}"></exception>
        public void IsTrue(string message = null)
        {
            if (!_func(_value))
            {
                // ReSharper disable once UnthrowableException
                throw _factory.Build(
                    new ExpressionCheckFailure<T>(CheckType.True, _value, default(T), message, _expression));
            }
        }

        /// <summary>
        ///     Checks the current value is false.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <exception cref="ExpressionCheckFailure{T}"></exception>
        public void IsFalse(string message = null)
        {
            if (_func(_value))
            {
                // ReSharper disable once UnthrowableException
                throw _factory.Build(new ExpressionCheckFailure<T>(CheckType.False, _value, default(T), message,
                    _expression));
            }
        }
    }
}