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

using Leoxia.Testing.Assertions.Abstractions;
using Leoxia.Testing.Assertions.Failures;
using Leoxia.Testing.Reflection;

#endregion

namespace Leoxia.Testing.Assertions
{
    /// <summary>
    ///     Checks for <see cref="object" />
    /// </summary>
    /// <typeparam name="T">type of expected value</typeparam>
    /// <seealso cref="Leoxia.Testing.Assertions.BaseClassCheckable{T}" />
    public class ObjectCheckable<T> : BaseClassCheckable<T>
        where T : class
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ObjectCheckable{T}" /> class.
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="value"></param>
        public ObjectCheckable(IExceptionFactory factory, T value) : base(factory, value)
        {
        }

        /// <summary>
        ///     Checks the Inner is equal to
        /// </summary>
        /// <param name="expected">The expected.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        /// <exception cref="ObjectCheckFailure"></exception>
        protected override bool InnerIsEqualTo(T expected, string message = null)
        {
            var trace = new CheckingTrace();
            if (!ObjectComparer.ObjectsAreEqual(_value, expected, PropertiesComparisonOptions.Default, trace))
            {
                // ReSharper disable once UnthrowableException
                throw _factory.Build(new ObjectCheckFailure(CheckType.Equal, _value, expected, trace, message));
            }
            return true;
        }

        /// <summary>
        ///     Checks the Inner is not equal to
        /// </summary>
        /// <param name="expected">The expected.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        /// <exception cref="ObjectCheckFailure"></exception>
        protected override bool InnerIsNotEqualTo(T expected, string message = null)
        {
            var trace = new CheckingTrace();
            if (ObjectComparer.ObjectsAreEqual(_value, expected, PropertiesComparisonOptions.Default, trace))
            {
                // ReSharper disable once UnthrowableException
                throw _factory.Build(new ObjectCheckFailure(CheckType.NotEqual, _value, expected, trace, message));
            }
            return true;
        }
    }
}