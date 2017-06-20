﻿#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseCheckFailure.cs" company="Leoxia Ltd">
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

#endregion

namespace Leoxia.Testing.Assertions.Failures
{
    /// <summary>
    ///     Base class for displaying the check failure message.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Leoxia.Testing.Assertions.ICheckFailure{IList}" />
    public abstract class BaseCheckFailure<T> : ICheckFailure<IList>
    {
        /// <summary>
        ///     The expected
        /// </summary>
        protected readonly T _expected;

        /// <summary>
        ///     The message
        /// </summary>
        protected readonly string _message;

        /// <summary>
        ///     The tested
        /// </summary>
        protected readonly T _tested;

        /// <summary>
        ///     The type
        /// </summary>
        protected readonly CheckType _type;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BaseCheckFailure{T}" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="tested">The tested.</param>
        /// <param name="expected">The expected.</param>
        /// <param name="message">The message.</param>
        protected BaseCheckFailure(CheckType type, T tested, T expected, string message)
        {
            _type = type;
            _tested = tested;
            _expected = expected;
            _message = message;
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            var formattableString = DisplayMessage();
            if (!string.IsNullOrEmpty(_message))
            {
                formattableString += Environment.NewLine + _message;
            }
            return formattableString;
        }

        /// <summary>
        ///     Displays the message.
        /// </summary>
        /// <returns></returns>
        protected abstract string DisplayMessage();
    }
}