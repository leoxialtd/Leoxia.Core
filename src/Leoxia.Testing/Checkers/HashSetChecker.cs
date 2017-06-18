#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HashSetChecker.cs" company="Leoxia Ltd">
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
using Leoxia.Collections;
using Leoxia.Testing.Assertions;

#endregion

namespace Leoxia.Testing.Checkers
{
    /// <summary>
    ///     Check that a <see cref="ISet{T}" /> respects its inherent contract.
    /// </summary>
    /// <typeparam name="T">type of elements</typeparam>
    /// <seealso cref="Leoxia.Testing.Checkers.SetChecker{T}" />
    public abstract class HashSetChecker<T> : SetChecker<T>
    {
        private readonly IHashSet<T> _hashSet;

        /// <summary>
        ///     Initializes a new instance of the <see cref="EnumerableChecker{T}" /> class.
        /// </summary>
        /// <param name="hashSet">The hash set.</param>
        protected HashSetChecker(IHashSet<T> hashSet) : base(hashSet)
        {
            _hashSet = hashSet;
        }

        /// <summary>
        ///     Sets the inheritor check.
        /// </summary>
        protected override void SetInheritorCheck()
        {
            var clone = _hashSet.Clone();
            foreach (var item in clone)
            {
                Check.That(_hashSet.Contains(item)).IsTrue();
            }
        }
    }
}