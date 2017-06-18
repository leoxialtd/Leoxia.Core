#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CollectionChecker.cs" company="Leoxia Ltd">
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
using Leoxia.Testing.Assertions;

#endregion

namespace Leoxia.Testing.Checkers
{
    /// <summary>
    ///     Check that a <see cref="ICollection{T}" /> respect its inherent contract.
    /// </summary>
    /// <typeparam name="T">type of elements</typeparam>
    /// <seealso cref="Leoxia.Testing.Checkers.EnumerableChecker{T}" />
    public abstract class CollectionChecker<T> : EnumerableChecker<T>
    {
        private readonly ICollection<T> _collection;

        /// <summary>
        ///     Initializes a new instance of the <see cref="EnumerableChecker{T}" /> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        protected CollectionChecker(ICollection<T> collection) : base(collection)
        {
            _collection = collection;
        }

        /// <summary>
        ///     Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        public abstract bool IsReadOnly { get; }

        /// <summary>
        ///     Check the inheritors of <see cref="EnumerableChecker{T}" />
        /// </summary>
        protected override void EnumerableInheritorCheck()
        {
            CollectionInheritorCheck();
            var count = _collection.Count;
            var newItem = AddNew();
            Check.That(_collection.Count).IsEqualTo(count + 1);
            if (IsReadOnly)
            {
                Check.That(_collection.IsReadOnly).IsTrue();
            }
            else
            {
                Check.That(_collection.IsReadOnly).IsFalse();
            }
            Check.That(_collection.Remove(newItem)).IsTrue();
            Check.That(_collection.Count).IsEqualTo(count);
            AddNew();
            AddNew();
            Check.That(_collection.Count).IsEqualTo(count + 2);
            var array = new T[_collection.Count];
            _collection.CopyTo(array, 0);
            foreach (var item in array)
            {
                Check.That(_collection.Contains(item)).IsTrue();
            }
            Check.That(_collection.Count).IsEqualTo(array.Length);
            _collection.Clear();
            Check.That(_collection.Count).IsEqualTo(0);
            CollectionInheritorCheck();
        }

        private T AddNew()
        {
            var newItem = GetNewItem();
            _collection.Add(newItem);
            Check.That(_collection.Contains(newItem)).IsTrue();
            return newItem;
        }

        /// <summary>
        ///     Provides a new item.
        /// </summary>
        /// <returns></returns>
        protected abstract T GetNewItem();

        /// <summary>
        ///     Check the inheritors of <see cref="CollectionChecker{T}" />
        /// </summary>
        protected abstract void CollectionInheritorCheck();
    }
}