#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EqualCheckFailure.cs" company="Leoxia Ltd">
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

#endregion

namespace Leoxia.Testing.Assertions.Failures
{
    /// <summary>
    ///     Display message on check failure for equality.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="Leoxia.Testing.Assertions.Failures.BaseCheckFailure{T}" />
    public class EqualCheckFailure<T> : BaseCheckFailure<T>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="EqualCheckFailure{T}" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="tested">The tested.</param>
        /// <param name="expected">The expected.</param>
        /// <param name="message">The message.</param>
        public EqualCheckFailure(CheckType type, T tested, T expected, string message) : base(type, tested, expected,
            message)
        {
        }

        /// <summary>
        ///     Displays the message.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        protected override string DisplayMessage()
        {
            switch (_type)
            {
                case CheckType.Equal:
                    return $"Check that {Display(_tested)} is equal to {Display(_expected)}: failure";
                case CheckType.NotEqual:
                    return $"Check that {Display(_tested)} is not equal to {Display(_expected)}: failure";
                case CheckType.OpEqual:
                    return $"Check that {Display(_tested)} == {Display(_expected)}: failure";
                case CheckType.OpNotEqual:
                    return $"Check that {Display(_tested)} != {Display(_expected)}: failure";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private string Display(T value)
        {
            var type = typeof(T);
            if (type == typeof(DateTime))
            {
                var date = (DateTime) (object) value;
                return date.ToString("dd/MM/YYYY hh:mm:ss,fff");
            }
            return value.ToString();
        }
    }
}