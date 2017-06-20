#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ListItemCheckFailure.cs" company="Leoxia Ltd">
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

using System.Collections.Generic;
using System.Linq;

#endregion

namespace Leoxia.Testing.Assertions.Failures
{
    /// <summary>
    ///     Display message for check failure on item of <see cref="IList{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Leoxia.Testing.Assertions.Failures.BaseCheckFailure{T}" />
    public class ListItemCheckFailure<T> : BaseCheckFailure<T>
    {
        private new readonly IList<T> _tested;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ListItemCheckFailure{T}" /> class.
        /// </summary>
        /// <param name="checkType">Type of the check.</param>
        /// <param name="tested">The tested.</param>
        /// <param name="expected">The expected.</param>
        /// <param name="message">The message.</param>
        public ListItemCheckFailure(CheckType checkType, IList<T> tested, T expected, string message)
            : base(checkType, default(T), expected, message)
        {
            _tested = tested;
        }

        /// <summary>
        ///     Displays the message.
        /// </summary>
        /// <returns></returns>
        protected override string DisplayMessage()
        {
            return DisplayItem(_expected) + " is not contained in [" + DisplayList() + "]";
        }

        private string DisplayList()
        {
            return string.Join(", ", _tested.Select(DisplayItem));
        }

        private static string DisplayItem(T x)
        {
            if (x == null)
            {
                return "Null";
            }
            var res = x.ToString();
            if (res == null)
            {
                return "Null";
            }
            if (res.Length == 0)
            {
                return "Empty";
            }
            return res;
        }
    }
}