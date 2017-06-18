#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SetChecker.cs" company="Leoxia Ltd">
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
    ///     Check that a <see cref="ISet{T}" /> respects its inherent contract.
    /// </summary>
    /// <typeparam name="T">type of elements</typeparam>
    /// <seealso cref="Leoxia.Testing.Checkers.CollectionChecker{T}" />
    public abstract class SetChecker<T> : CollectionChecker<T>
    {
        private readonly ISet<T> _set;

        /// <summary>
        ///     Initializes a new instance of the <see cref="EnumerableChecker{T}" /> class.
        /// </summary>
        /// <param name="set">The set.</param>
        protected SetChecker(ISet<T> set) : base(set)
        {
            _set = set;
        }

        /// <summary>
        ///     Check inheritors of <see cref="SetChecker{T}" />.
        /// </summary>
        protected abstract void SetInheritorCheck();

        /// <summary>
        ///     Check the inheritors of <see cref="CollectionChecker{T}" />
        /// </summary>
        // ReSharper disable once MethodTooLong
        protected override void CollectionInheritorCheck()
        {
            SetInheritorCheck();
            _set.Clear();
            var list = CreateList();
            var item = GetNewItem();
            _set.Add(item);
            _set.ExceptWith(list);
            Check.That(_set.Count).IsEqualTo(1);
            Check.That(_set.Contains(item)).IsTrue();
            var properSuperset = CreateList();
            var smallerSet = new List<T>(properSuperset);
            smallerSet.RemoveAt(smallerSet.Count - 1);
            _set.IntersectWith(smallerSet);
            Check.That(_set.Count).IsEqualTo(smallerSet.Count);

            Check.That(_set.IsProperSubsetOf(properSuperset)).IsTrue();
            Check.That(_set.IsSubsetOf(properSuperset)).IsTrue();

            var superSet = new List<T>(_set);
            Check.That(_set.SetEquals(superSet)).IsTrue();
            Check.That(_set.IsProperSubsetOf(superSet)).IsFalse();
            Check.That(_set.IsSubsetOf(superSet)).IsTrue();

            Check.That(_set.IsProperSupersetOf(superSet)).IsFalse();
            Check.That(_set.IsSupersetOf(superSet)).IsTrue();

            var properSubSet = new List<T>(_set);
            properSubSet.RemoveAt(properSubSet.Count - 1);

            Check.That(_set.IsProperSupersetOf(properSubSet)).IsTrue();
            Check.That(_set.IsSupersetOf(properSubSet)).IsTrue();

            var newList = CreateList(false);

            Check.That(_set.Overlaps(superSet)).IsTrue();
            Check.That(_set.Overlaps(properSubSet)).IsTrue();
            Check.That(_set.Overlaps(properSuperset)).IsTrue();
            Check.That(_set.Overlaps(newList)).IsFalse();

            var count = _set.Count;
            _set.UnionWith(properSubSet);
            Check.That(_set.Count).IsEqualTo(count);
            _set.UnionWith(superSet);
            Check.That(_set.Count).IsEqualTo(count);
            _set.UnionWith(properSuperset);
            Check.That(_set.Count).IsEqualTo(properSuperset.Count);
            count = _set.Count;
            _set.UnionWith(newList);
            Check.That(_set.Count).IsEqualTo(count + newList.Count);

            var l = new List<T>(_set);
            l.Add(GetNewItem());
            _set.Add(GetNewItem());
            _set.SymmetricExceptWith(l);
            Check.That(l.Count).IsEqualTo(1);
            Check.That(_set.Count).IsEqualTo(1);

            SetInheritorCheck();
        }

        // ReSharper disable once FlagArgument
        private List<T> CreateList(bool added = true)
        {
            var list = new List<T>();
            for (var i = 0; i < 10; i++)
            {
                var item = GetNewItem();
                list.Add(item);
                if (added)
                {
                    _set.Add(item);
                }
            }
            return list;
        }
    }
}