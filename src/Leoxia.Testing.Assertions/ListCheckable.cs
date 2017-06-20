#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListCheckable.cs" company="Leoxia Ltd">
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

using System.Collections;
using System.Collections.Generic;
using Leoxia.Testing.Assertions.Abstractions;
using Leoxia.Testing.Assertions.Failures;

#endregion

namespace Leoxia.Testing.Assertions
{
    /// <summary>
    ///     Checks for <see cref="IList" />
    /// </summary>
    /// <seealso cref="Leoxia.Testing.Assertions.BaseClassCheckable{IList}" />
    /// <seealso cref="Leoxia.Testing.Assertions.Abstractions.IListCheckable" />
    public class ListCheckable : BaseClassCheckable<IList>, IListCheckable
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ListCheckable" /> class.
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="value"></param>
        public ListCheckable(IExceptionFactory factory, IList value)
            : base(factory, value)
        {
        }

        /// <summary>
        ///     Checks the Inner is equal to
        /// </summary>
        /// <param name="expected">The expected.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        protected override bool InnerIsEqualTo(IList expected, string message = null)
        {
            if (_value.Count != expected.Count)
            {
                var checkFailure = new ListCheckFailure<IList>(CheckType.CountEqual, _value, expected, message);
                checkFailure.ExpectedCount = expected.Count;
                checkFailure.TestedCount = _value.Count;
                // ReSharper disable once UnthrowableException
                throw _factory.Build(checkFailure);
            }
            for (var i = 0; i < _value.Count; ++i)
            {
                if (_value[i] != expected[i])
                {
                    var checkFailure = new ListCheckFailure<IList>(CheckType.ListItemEqual, _value, expected, message);
                    checkFailure.Index = i;
                    // ReSharper disable once UnthrowableException
                    throw _factory.Build(checkFailure);
                }
            }
            return true;
        }

        /// <summary>
        ///     Checks the Inner is not equal to
        /// </summary>
        /// <param name="expected">The expected.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        protected override bool InnerIsNotEqualTo(IList expected, string message = null)
        {
            if (_value.Count != expected.Count)
            {
                return true;
            }
            for (var i = 0; i < _value.Count; ++i)
            {
                if (_value[i] != expected[i])
                {
                    return true;
                }
            }
            var checkFailure = new ListCheckFailure<IList>(CheckType.ListItemNotEqual, _value, expected, message);
            // ReSharper disable once UnthrowableException
            throw _factory.Build(checkFailure);
        }
    }

    /// <summary>
    ///     Checks for <see cref="IList{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Assertions.BaseClassCheckable{IList}" />
    /// <seealso cref="Leoxia.Testing.Assertions.Abstractions.IListCheckable" />
    public class ListCheckable<T> : BaseClassCheckable<IList<T>>, IListCheckable<T>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ListCheckable{T}" /> class.
        /// </summary>
        /// <param name="factory"></param>
        /// <param name="value"></param>
        public ListCheckable(IExceptionFactory factory, IList<T> value) : base(factory, value)
        {
        }

        /// <summary>
        ///     Checks that the item is contained by the list.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="message">The message.</param>
        public void Contains(T item, string message = null)
        {
            if (!_value.Contains(item))
            {
                var checkFailure = new ListItemCheckFailure<T>(CheckType.ListItemIsNotContained, _value, item, message);
                // ReSharper disable once UnthrowableException
                throw _factory.Build(checkFailure);
            }
        }

        /// <summary>
        ///     Checks the Inner is equal to
        /// </summary>
        /// <param name="expected">The expected.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        protected override bool InnerIsEqualTo(IList<T> expected, string message = null)
        {
            if (_value.Count != expected.Count)
            {
                var checkFailure = new ListCheckFailure<IList<T>>(CheckType.CountEqual, _value, expected, message);
                checkFailure.ExpectedCount = expected.Count;
                checkFailure.TestedCount = _value.Count;
                // ReSharper disable once UnthrowableException
                throw _factory.Build(checkFailure);
            }
            for (var i = 0; i < _value.Count; ++i)
            {
                if (!_value[i].Equals(expected[i]))
                {
                    var checkFailure =
                        new ListCheckFailure<IList<T>>(CheckType.ListItemEqual, _value, expected, message);
                    checkFailure.Index = i;
                    // ReSharper disable once UnthrowableException
                    throw _factory.Build(checkFailure);
                }
            }
            return true;
        }

        /// <summary>
        ///     Checks the Inner is not equal to
        /// </summary>
        /// <param name="expected">The expected.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        protected override bool InnerIsNotEqualTo(IList<T> expected, string message = null)
        {
            if (_value.Count == expected.Count)
            {
                return true;
            }
            for (var i = 0; i < _value.Count; ++i)
            {
                if (!_value[i].Equals(expected[i]))
                {
                    return true;
                }
            }
            var checkFailure = new ListCheckFailure<IList<T>>(CheckType.ListItemAllEqual, _value, expected, message);
            // ReSharper disable once UnthrowableException
            throw _factory.Build(checkFailure);
        }
    }
}