﻿#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExceptionFactory.cs" company="Leoxia Ltd">
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
using Leoxia.Testing.Assertions.Abstractions;

#endregion

namespace Leoxia.Testing.Assertions
{
    /// <summary>
    ///     Builds <see cref="Exception" />
    /// </summary>
    /// <seealso cref="Leoxia.Testing.Assertions.Abstractions.IExceptionFactory" />
    public class ExceptionFactory : IExceptionFactory
    {
        /// <summary>
        ///     Builds the specified <see cref="Exception" />.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="checkable">The checkable.</param>
        /// <returns></returns>
        public Exception Build<T>(ICheckFailure<T> checkable)
        {
            return new AssertionException(checkable.ToString());
        }
    }
}