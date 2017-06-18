#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableExtensions.cs" company="Leoxia Ltd">
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
using System.Collections.Generic;

#endregion

namespace Leoxia.Collections
{
    /// <summary>
    ///     Extensions method for collections
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        ///     Convert enumerable to a dictionary.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <typeparam name="TInput">The type of the input.</typeparam>
        /// <param name="input">The input.</param>
        /// <param name="keyGetter">The key getter.</param>
        /// <param name="valueGetter">The value getter.</param>
        /// <returns>a dictionary of key values</returns>
        public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue, TInput>(this IEnumerable<TInput> input,
            Func<TInput, TKey> keyGetter, Func<TInput, TValue> valueGetter)
        {
            var dictionary = new Dictionary<TKey, TValue>();
            foreach (var item in input)
            {
                dictionary.Add(keyGetter(item), valueGetter(item));
            }
            return dictionary;
        }

        /// <summary>
        ///     Gets the cursor on the given <see cref="IEnumerable{T}" />.
        /// </summary>
        /// <typeparam name="T">type of elements</typeparam>
        /// <param name="enumerable">The enumerable.</param>
        /// <returns>cursor</returns>
        public static ICursor<T> GetCursor<T>(this IEnumerable<T> enumerable) where T : class
        {
            return new Cursor<T>(enumerable);
        }
    }
}