#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICheckFailureMessageFactory.cs" company="Leoxia Ltd">
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

using Leoxia.Testing.Reflection;

#endregion

namespace Leoxia.Testing.Assertions.Abstractions
{
    /// <summary>
    ///     Factory building failure messages
    /// </summary>
    public interface ICheckFailureMessageFactory
    {
        /// <summary>
        ///     Builds the message.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="checkType">Type of the check.</param>
        /// <param name="tested">The tested.</param>
        /// <param name="expected">The expected.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        string BuildMessage<T>(CheckType checkType, T tested, T expected, string message);

        /// <summary>
        ///     Builds the message.
        /// </summary>
        /// <param name="checkType">Type of the check.</param>
        /// <param name="trace">The trace.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        string BuildMessage(CheckType checkType, CheckingTrace trace, string message);
    }
}