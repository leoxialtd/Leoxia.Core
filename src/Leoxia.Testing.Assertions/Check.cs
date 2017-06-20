#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Check.cs" company="Leoxia Ltd">
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
using System.Collections.Generic;
using Leoxia.Testing.Assertions.Abstractions;

#endregion

namespace Leoxia.Testing.Assertions
{
    /// <summary>
    /// Entry class for fluent assertions.
    /// </summary>
    public static class Check
    {
        private static readonly IExceptionFactory _factory;

        /// <summary>
        /// Initializes the <see cref="Check"/> class.
        /// </summary>
        static Check()
        {
            _factory = new ExceptionFactory();
        }

        /// <summary>
        /// Check on the specified value.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        public static IBoolCheckable That(bool value)
        {
            return new BoolCheckable(_factory, value);
        }

        /// <summary>
        /// Check on the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static IIntegerCheckable That(int value)
        {
            return new IntegerCheckable(_factory, value);
        }

        /// <summary>
        /// Check on the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static IDateTimeCheckable That(DateTime value)
        {
            return new DateTimeCheckable(_factory, value);
        }

        /// <summary>
        /// Check on the specified value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static IListCheckable<T> That<T>(IList<T> value)
        {
            return new ListCheckable<T>(_factory, value);
        }

        /// <summary>
        /// Check on the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static IStringCheckable That(string value)
        {
            return new StringCheckable(_factory, value);
        }

        /// <summary>
        /// Check on the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">value</exception>
        public static IListCheckable ThatList(IList value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            return new ListCheckable(_factory, value);
        }

        /// <summary>
        /// Check on the specified value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static IEquatableCheckable<T> That<T>(IEquatable<T> value)
            where T : IEquatable<T>
        {
            return new EquatableCheckable<T>(_factory, value);
        }

        /// <summary>
        /// Check on the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static ITypeCheckable That(Type value)
        {
            return new TypeCheckable(_factory, value);
        }

        /// <summary>
        /// Check on the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static IClassCheckable<object> ThatObject(object value)
        {
            return new ObjectCheckable<object>(_factory, value);
        }
    }
}