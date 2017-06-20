#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringCheckFailure.cs" company="Leoxia Ltd">
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
    ///     Display message for check failures on <see cref="string" />
    /// </summary>
    /// <seealso cref="Leoxia.Testing.Assertions.Failures.BaseCheckFailure{String}" />
    public class StringCheckFailure : BaseCheckFailure<string>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="StringCheckFailure" /> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="tested">The tested.</param>
        /// <param name="expected">The expected.</param>
        /// <param name="message">The message.</param>
        public StringCheckFailure(CheckType type, string tested, string expected, string message)
            : base(type, tested, expected, message)
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
                case CheckType.StringNotNullOrEmpty:
                {
                    var value = string.Empty;
                    if (_tested == null)
                    {
                        value = "null";
                    }
                    else if (_tested.Length == 0)
                    {
                        value = "empty";
                    }
                    return $"Checking that tested string is neither null nor empty but it is {value}";
                }
                case CheckType.StringNullOrEmpty:
                    return
                        $"Checking that tested value [{_tested}] is null or empty: failure, Tested.Length = {_tested.Length}";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}