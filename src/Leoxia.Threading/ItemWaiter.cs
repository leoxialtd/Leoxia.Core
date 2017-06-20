#region Copyright (c) 2017 Leoxia Ltd

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ItemWaiter.cs" company="Leoxia Ltd">
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
using System.Collections.Concurrent;
using System.Threading;

#endregion

namespace Leoxia.Threading
{
    /// <summary>
    ///     Wait for item be pushed.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TItem">The type of the item.</typeparam>
    public class ItemWaiter<TKey, TItem>
    {
        private readonly ConcurrentDictionary<TKey, TItem> _dictionary = new ConcurrentDictionary<TKey, TItem>();
        private readonly Func<TItem, TKey> _function;
        private TItem _last;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public ItemWaiter(Func<TItem, TKey> function)
        {
            _function = function;
        }

        /// <summary>
        ///     Pushes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public void Push(TItem item)
        {
            _last = item;
            var key = _function(item);
            _dictionary.AddOrUpdate(key, item, (x, y) => y);
        }

        /// <summary>
        ///     Waits the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public TItem Wait(TKey key)
        {
            TItem item;
            var count = 0;
            while (!_dictionary.TryGetValue(key, out item) && count < 100)
            {
                Thread.Sleep(50);
                count++;
            }
            return item;
        }

        /// <summary>
        ///     Gets the last.
        /// </summary>
        /// <returns></returns>
        public TItem GetLast()
        {
            return _last;
        }
    }
}