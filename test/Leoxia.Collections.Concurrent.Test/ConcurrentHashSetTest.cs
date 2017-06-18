#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConcurrentHashSetTest.cs" company="Leoxia Ltd">
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
using Leoxia.Testing.Checkers;
using Xunit;

#endregion

namespace Leoxia.Collections.Concurrent.Test
{
    public class ConcurrentHashSetChecker<T> : HashSetChecker<T>
    {
        private readonly Func<T> _generator;

        /// <summary>
        ///     Initializes a new instance of the <see cref="EnumerableChecker{T}" /> class.
        /// </summary>
        /// <param name="hashSet">The hash set.</param>
        /// <param name="generator">The generator.</param>
        public ConcurrentHashSetChecker(IHashSet<T> hashSet, Func<T> generator) : base(hashSet)
        {
            _generator = generator;
        }

        /// <summary>
        ///     Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        public override bool IsReadOnly => false;

        /// <summary>
        ///     Checks the element at the given index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="item">The item.</param>
        protected override void CheckElement(int index, T item)
        {
        }

        /// <summary>
        ///     Provides a new item.
        /// </summary>
        /// <returns></returns>
        protected override T GetNewItem()
        {
            return _generator();
        }
    }

    /// <summary>
    ///     Test of <see cref="ConcurrentHashSet{T}" />
    /// </summary>
    public class ConcurrentHashSetTest
    {
        private int _increment;

        /// <summary>
        ///     Checks the hash set.
        /// </summary>
        [Fact]
        public void CheckSet()
        {
            var hashSet = new ConcurrentHashSet<string>();
            var hashSetChecker = new ConcurrentHashSetChecker<string>(hashSet, GetNewString);
            hashSetChecker.CheckInterface();
        }

        private string GetNewString()
        {
            _increment++;
            return _increment.ToString();
        }
    }
}