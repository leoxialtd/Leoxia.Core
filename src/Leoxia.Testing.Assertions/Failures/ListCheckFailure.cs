#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListCheckFailure.cs" company="Leoxia Ltd">
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

#endregion

namespace Leoxia.Testing.Assertions.Failures
{
    /// <summary>
    ///     Display for message checks failures on <see cref="IList{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Leoxia.Testing.Assertions.Failures.BaseCheckFailure{T}" />
    public class ListCheckFailure<T> : BaseCheckFailure<T>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ListCheckFailure{T}" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="tested">The tested.</param>
        /// <param name="expected">The expected.</param>
        /// <param name="message">The message.</param>
        public ListCheckFailure(CheckType type, T tested, T expected, string message) : base(type, tested, expected,
            message)
        {
        }

        /// <summary>
        ///     Gets or sets the expected count.
        /// </summary>
        /// <value>
        ///     The expected count.
        /// </value>
        public int ExpectedCount { get; set; }

        /// <summary>
        ///     Gets or sets the index.
        /// </summary>
        /// <value>
        ///     The index.
        /// </value>
        public int Index { get; set; }

        /// <summary>
        ///     Gets or sets the tested count.
        /// </summary>
        /// <value>
        ///     The tested count.
        /// </value>
        public int TestedCount { get; set; }

        /// <summary>
        ///     Displays the message.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        protected override string DisplayMessage()
        {
            switch (_type)
            {
                case CheckType.CountEqual:
                {
                    return $"Check that {_tested} is equal to {_expected} but item count is different:" +
                           Environment.NewLine +
                           $"Tested.Count == {TestedCount} and Expected.Count == {ExpectedCount}";
                }
                case CheckType.ListItemEqual:
                {
                    return $"Check that {_tested} is equal to {_expected} but items are different on index {Index}:" +
                           Environment.NewLine +
                           $"Tested[{Index}] == {((IList) _tested)[Index]} and Expected[{Index}] == {((IList) _expected)[Index]}";
                }
                case CheckType.ListItemNotEqual:
                {
                    return $"Check that {_tested} is not equal to {_expected} but all items are equal";
                }
                default:
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}