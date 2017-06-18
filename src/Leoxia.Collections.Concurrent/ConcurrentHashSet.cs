#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConcurrentHashSet.cs" company="Leoxia Ltd">
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

using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Leoxia.Collections.Concurrent
{
    /// <summary>
    ///     Concurrent <see cref="HashSet{T}" />
    /// </summary>
    /// <typeparam name="T">type of elements</typeparam>
    /// <seealso cref="System.Collections.Generic.IEnumerable{T}" />
    public class ConcurrentHashSet<T> : IHashSet<T>
    {
        private readonly ConcurrentDictionary<T, int> _dictionary;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConcurrentHashSet{T}" /> class.
        /// </summary>
        public ConcurrentHashSet()
        {
            _dictionary = new ConcurrentDictionary<T, int>();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ConcurrentHashSet{T}" /> class.
        /// </summary>
        /// <param name="enumerable">The enumerable.</param>
        // ReSharper disable once MemberCanBePrivate.Global
        public ConcurrentHashSet(IEnumerable<T> enumerable)
        {
            _dictionary = new ConcurrentDictionary<T, int>(
                enumerable.Select(x => new KeyValuePair<T, int>(x, 0)));
        }

        /// <summary>Adds an element to the current set and returns a value to indicate if the element was successfully added. </summary>
        /// <returns>true if the element is added to the set; false if the element is already in the set.</returns>
        /// <param name="item">The element to add to the set.</param>
        // ReSharper disable once MethodNameNotMeaningful
        public void Add(T item)
        {
            InnerAdd(item);
        }

        /// <summary>Removes all elements in the specified collection from the current set.</summary>
        /// <param name="other">The collection of items to remove from the set.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="other" /> is null.
        /// </exception>
        public void ExceptWith(IEnumerable<T> other)
        {
            foreach (var item in other)
            {
                Remove(item);
            }
        }

        /// <summary>Modifies the current set so that it contains only elements that are also in a specified collection.</summary>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="other" /> is null.
        /// </exception>
        public void IntersectWith(IEnumerable<T> other)
        {
            var hash = new HashSet<T>(other);
            var keys = _dictionary.Keys.ToArray();
            foreach (var key in keys)
            {
                if (!hash.Contains(key))
                {
                    Remove(key);
                }
            }
        }

        /// <summary>Determines whether the current set is a proper (strict) subset of a specified collection.</summary>
        /// <returns>true if the current set is a proper subset of <paramref name="other" />; otherwise, false.</returns>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="other" /> is null.
        /// </exception>
        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            var hash = new HashSet<T>(other);
            return _dictionary.Keys.All(x => hash.Contains(x)) && hash.Count > _dictionary.Keys.Count;
        }

        /// <summary>Determines whether the current set is a proper (strict) superset of a specified collection.</summary>
        /// <returns>true if the current set is a proper superset of <paramref name="other" />; otherwise, false.</returns>
        /// <param name="other">The collection to compare to the current set. </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="other" /> is null.
        /// </exception>
        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            var list = other.ToList();
            return IsSupersetOf(list) && list.Count < _dictionary.Keys.Count;
        }

        /// <summary>Determines whether a set is a subset of a specified collection.</summary>
        /// <returns>true if the current set is a subset of <paramref name="other" />; otherwise, false.</returns>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="other" /> is null.
        /// </exception>
        public bool IsSubsetOf(IEnumerable<T> other)
        {
            var hash = new HashSet<T>(other);
            return _dictionary.Keys.All(x => hash.Contains(x));
        }

        /// <summary>Determines whether the current set is a superset of a specified collection.</summary>
        /// <returns>true if the current set is a superset of <paramref name="other" />; otherwise, false.</returns>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="other" /> is null.
        /// </exception>
        public bool IsSupersetOf(IEnumerable<T> other)
        {
            return other.All(x => _dictionary.ContainsKey(x));
        }

        /// <summary>Determines whether the current set overlaps with the specified collection.</summary>
        /// <returns>true if the current set and <paramref name="other" /> share at least one common element; otherwise, false.</returns>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="other" /> is null.
        /// </exception>
        public bool Overlaps(IEnumerable<T> other)
        {
            return other.Any(x => _dictionary.ContainsKey(x));
        }

        /// <summary>Determines whether the current set and the specified collection contain the same elements.</summary>
        /// <returns>true if the current set is equal to <paramref name="other" />; otherwise, false.</returns>
        /// <param name="other">The collection to compare to the current set.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="other" /> is null.
        /// </exception>
        public bool SetEquals(IEnumerable<T> other)
        {
            var set = new HashSet<T>(other);
            if (set.Count != _dictionary.Keys.Count)
            {
                return false;
            }
            return IsSupersetOf(set);
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
            var collection = other as ICollection<T>;
            if (collection == null)
            {
                collection = other.ToList();
            }
            var copy = new List<T>(collection);
            foreach (var item in copy)
            {
                if (_dictionary.ContainsKey(item))
                {
                    collection.Remove(item);
                    Remove(item);
                }
            }
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
            foreach (var item in other)
            {
                if (!_dictionary.ContainsKey(item))
                {
                    _dictionary[item] = 0;
                }
            }
        }

        /// <summary>Adds an element to the current set and returns a value to indicate if the element was successfully added. </summary>
        /// <returns>true if the element is added to the set; false if the element is already in the set.</returns>
        /// <param name="item">The element to add to the set.</param>
        // ReSharper disable once MethodNameNotMeaningful
        bool ISet<T>.Add(T item)
        {
            return InnerAdd(item);
        }

        /// <summary>Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1" />.</summary>
        /// <exception cref="T:System.NotSupportedException">
        ///     The <see cref="T:System.Collections.Generic.ICollection`1" /> is
        ///     read-only.
        /// </exception>
        public void Clear()
        {
            _dictionary.Clear();
        }

        /// <summary>Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.</summary>
        /// <returns>
        ///     true if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />;
        ///     otherwise, false.
        /// </returns>
        /// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
        public bool Contains(T item)
        {
            return _dictionary.ContainsKey(item);
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
            _dictionary.Keys.CopyTo(array, arrayIndex);
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
            int value;
            return _dictionary.TryRemove(item, out value);
        }

        /// <summary>Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.</summary>
        /// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1" />.</returns>
        public int Count => _dictionary.Count;

        /// <summary>Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only.</summary>
        /// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1" /> is read-only; otherwise, false.</returns>
        public bool IsReadOnly => false;

        /// <summary>
        ///     Clones this instance.
        /// </summary>
        /// <returns>clone of this instance</returns>
        public IHashSet<T> Clone()
        {
            return new ConcurrentHashSet<T>(this);
        }

        /// <summary>
        ///     Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        ///     An enumerator that can be used to iterate through the collection.
        /// </returns>
        public IEnumerator<T> GetEnumerator()
        {
            return _dictionary.Keys.GetEnumerator();
        }

        /// <summary>
        ///     Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        ///     An <see cref="T:System.Collections.IEnumerator" /> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private bool InnerAdd(T instance)
        {
            var res = _dictionary.ContainsKey(instance);
            _dictionary[instance] = 0;
            return res;
        }
    }
}