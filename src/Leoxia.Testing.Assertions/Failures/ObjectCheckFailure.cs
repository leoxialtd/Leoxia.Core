#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectCheckFailure.cs" company="Leoxia Ltd">
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
using Leoxia.Testing.Reflection;

#endregion

namespace Leoxia.Testing.Assertions.Failures
{
    /// <summary>
    ///     Display message failure for checks on <see cref="object" />
    /// </summary>
    /// <seealso cref="Leoxia.Testing.Assertions.Failures.BaseCheckFailure{Object}" />
    public class ObjectCheckFailure : BaseCheckFailure<object>
    {
        private readonly CheckingTrace _trace;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ObjectCheckFailure" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="tested">The tested.</param>
        /// <param name="expected">The expected.</param>
        /// <param name="trace">The trace.</param>
        /// <param name="message">The message.</param>
        public ObjectCheckFailure(CheckType type, object tested, object expected, CheckingTrace trace,
            string message) : base(type, tested, expected, message)
        {
            _trace = trace;
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
                {
                    return $"Check that {_tested} is equal to {_expected}: failure" + Environment.NewLine +
                           _trace;
                }
                case CheckType.NotEqual:
                {
                    return $"Check that {_tested} is not equal to {_expected}: failure" + Environment.NewLine +
                           _trace;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}