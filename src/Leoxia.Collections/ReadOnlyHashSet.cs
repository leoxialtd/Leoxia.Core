#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadOnlyHashSet.cs" company="Leoxia Ltd">
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
using System.Collections;
using System.Collections.Generic;

#endregion

namespace Leoxia.Collections
{
    /// <summary>
    ///     <see cref="HashSet{T}" /> which is read only.
    /// </summary>
    /// <typeparam name="T">type of element</typeparam>
    /// <seealso cref="Leoxia.Collections.IHashSet{T}" />
    public class ReadOnlyHashSet<T> : IHashSet<T>
    {
        private readonly HashSet<T> _inner;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ReadOnlyHashSet{T}" /> class.
        /// </summary>
        /// <param name="inner">The inner.</param>
        public ReadOnlyHashSet(HashSet<T> inner)
        {
            _inner = inner;
        }

        /// <summary>
        ///     Clones this instance.
        /// </summary>
        /// <returns>
        ///     clone of this instance
        /// </returns>
        public IHashSet<T> Clone()
        {
            return new ReadOnlyHashSet<T>(new HashSet<T>(_inner));
        }

        /// <summary>Returns an enumerator that iterates through the collection.</summary>
        /// <returns>An enumerator that can be used to iterate through the collection.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _inner.GetEnumerator();
        }

        /// <summary>Returns an enumerator that iterates through a collection.</summary>
        /// <returns>An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _inner).GetEnumerator();
        }

        /// <summary>Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1" />.</summary>
        /// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <exception cref="T:System.NotSupportedException">
        ///     The <see cref="T:System.Collections.Generic.ICollection`1" /> is
        ///     read-only.
        /// </exception>
        // ReSharper disable once MethodNameNotMeaningful
        public void Add(T item)
        {
            throw new InvalidOperationException($"Cannot add in a ReadOnlyHashSet<{typeof(T)}>");
        }

        /// <summary>Removes all elements in the specified collection from the current set.</summary>
        /// <param name="other">The collection of items to remove from the set.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="other" /> is null.
        /// </exception>
        public void ExceptWith(IEnumerable<T> other)
        {
            _inner.ExceptWith(other);
        }

        /// <summary>Modifies the current set so that it contains only elements that are also in a specified collection.</summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="other" /> is null.
        /// </exception>
        public void IntersectWith(IEnumerable<T> other)
        {
            _inner.IntersectWith(other);
        }

        /// <summary>Determines whether the current set is a proper (strict) subset of a specified collection.</summary>
        /// <returns>true if the current set is a proper subset of <paramref name="other" />; otherwise, false.</returns>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="other" /> is null.
        /// </exception>
        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            return _inner.IsProperSubsetOf(other);
        }

        /// <summary>Determines whether the current set is a proper (strict) superset of a specified collection.</summary>
        /// <returns>true if the current set is a proper superset of <paramref name="other" />; otherwise, false.</returns>
        /// <param name="other">The collection to compare to the current set. </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="other" /> is null.
        /// </exception>
        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            return _inner.IsProperSupersetOf(other);
        }

        /// <summary>Determines whether a set is a subset of a specified collection.</summary>
        /// <returns>true if the current set is a subset of <paramref name="other" />; otherwise, false.</returns>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="other" /> is null.
        /// </exception>
        public bool IsSubsetOf(IEnumerable<T> other)
        {
            return _inner.IsSubsetOf(other);
        }

        /// <summary>Determines whether the current set is a superset of a specified collection.</summary>
        /// <returns>true if the current set is a superset of <paramref name="other" />; otherwise, false.</returns>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="other" /> is null.
        /// </exception>
        public bool IsSupersetOf(IEnumerable<T> other)
        {
            return _inner.IsSupersetOf(other);
        }

        /// <summary>Determines whether the current set overlaps with the specified collection.</summary>
        /// <returns>true if the current set and <paramref name="other" /> share at least one common element; otherwise, false.</returns>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="other" /> is null.
        /// </exception>
        public bool Overlaps(IEnumerable<T> other)
        {
            return _inner.Overlaps(other);
        }

        /// <summary>Determines whether the current set and the specified collection contain the same elements.</summary>
        /// <returns>true if the current set is equal to <paramref name="other" />; otherwise, false.</returns>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="other" /> is null.
        /// </exception>
        public bool SetEquals(IEnumerable<T> other)
        {
            return _inner.SetEquals(other);
        }

        /// <summary>
        ///     Modifies the current set so that it contains only elements that are present either in the current set or in
        ///     the specified collection, but not both.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="other" /> is null.
        /// </exception>
        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            _inner.SymmetricExceptWith(other);
        }

        /// <summary>
        ///     Modifies the current set so that it contains all elements that are present in the current set, in the
        ///     specified collection, or in both.
        /// </summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="other" /> is null.
        /// </exception>
        public void UnionWith(IEnumerable<T> other)
        {
            _inner.UnionWith(other);
        }

        /// <summary>Adds an element to the current set and returns a value to indicate if the element was successfully added. </summary>
        /// <returns>true if the element is added to the set; false if the element is already in the set.</returns>
        /// <param name="item">The element to add to the set.</param>
        // ReSharper disable once MethodNameNotMeaningful
        bool ISet<T>.Add(T item)
        {
            throw new InvalidOperationException($"Cannot add in a ReadOnlyHashSet<{typeof(T)}>");
        }

        /// <summary>Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.</summary>
        /// <exception cref="T:System.NotSupportedException">
        ///     The <see cref="T:System.Collections.Generic.ICollection`1" /> is
        ///     read-only.
        /// </exception>
        public void Clear()
        {
            throw new InvalidOperationException($"Cannot clear a ReadOnlyHashSet<{typeof(T)}>");
        }

        /// <summary>Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.</summary>
        /// <returns>
        ///     true if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />;
        ///     otherwise, false.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        public bool Contains(T item)
        {
            return _inner.Contains(item);
        }

        /// <summary>
        ///     Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1" /> to an
        ///     <see cref="T:System.Array" />, starting at a particular <see cref="T:System.Array" /> index.
        /// </summary>
        /// <param name="array">
        ///     The one-dimensional <see cref="T:System.Array" /> that is the destination of the elements copied
        ///     from <see cref="T:System.Collections.Generic.ICollection`1" />. The <see cref="T:System.Array" /> must have
        ///     zero-based indexing.
        /// </param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array" /> at which copying begins.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="array" /> is null.
        /// </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///     <paramref name="arrayIndex" /> is less than 0.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///     The number of elements in the source
        ///     <see cref="T:System.Collections.Generic.ICollection`1" /> is greater than the available space from
        ///     <paramref name="arrayIndex" /> to the end of the destination <paramref name="array" />.
        /// </exception>
        public void CopyTo(T[] array, int arrayIndex)
        {
            _inner.CopyTo(array, arrayIndex);
        }

        /// <summary>
        ///     Removes the first occurrence of a specific object from the
        ///     <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </summary>
        /// <returns>
        ///     true if <paramref name="item" /> was successfully removed from the
        ///     <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false. This method also returns false if
        ///     <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.
        /// </returns>
        /// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        /// <exception cref="T:System.NotSupportedException">
        ///     The <see cref="T:System.Collections.Generic.ICollection`1" /> is
        ///     read-only.
        /// </exception>
        public bool Remove(T item)
        {
            throw new InvalidOperationException($"Cannot remove from a ReadOnlyHashSet<{typeof(T)}>");
        }

        /// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.</summary>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.</returns>
        int ICollection<T>.Count => _inner.Count;

        /// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.</summary>
        /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only; otherwise, false.</returns>
        public bool IsReadOnly => true;
    }
}