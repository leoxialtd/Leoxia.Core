#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICheckable.cs" company="Leoxia Ltd">
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

#endregion

namespace Leoxia.Testing.Assertions.Abstractions
{
    /// <summary>
    ///     Interfaces for checks
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICheckable<T>
    {
        /// <summary>
        ///     Checks whether the current value respects the contract
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <returns></returns>
        IBoolCheckable Is(Expression<Func<T, bool>> contract);

        /// <summary>
        ///     Checks that the current value is equal to the expected value.
        /// </summary>
        /// <param name="expected">The expected.</param>
        /// <param name="message">The message.</param>
        void IsEqualTo(T expected, string message = null);

        /// <summary>
        ///     Checks that the current value is not equal to the expected value.
        /// </summary>
        /// <param name="expected">The expected.</param>
        /// <param name="message">The message.</param>
        void IsNotEqualTo(T expected, string message = null);

        /// <summary>
        ///     Checks that the current value is operator equal to the expected value.
        /// </summary>
        /// <param name="expected">The expected.</param>
        /// <param name="message">The message.</param>
        void IsOperatorEqualTo(T expected, string message = null);

        /// <summary>
        ///     Checks that the current value is not operator equal to the expected value.
        /// </summary>
        /// <param name="expected">The expected.</param>
        /// <param name="message">The message.</param>
        void IsOperatorNotEqualTo(T expected, string message = null);
    }
}