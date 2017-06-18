using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Leoxia.Threading
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TItem">The type of the item.</typeparam>
    public class ItemWaiter<TKey, TItem>
    {
        private readonly Func<TItem, TKey> _function;
        private readonly ConcurrentDictionary<TKey, TItem> _dictionary = new ConcurrentDictionary<TKey, TItem>();
        private TItem _last;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public ItemWaiter(Func<TItem, TKey> function)
        {
            _function = function;
        }

        public void Push(TItem item)
        {
            _last = item;
            var key = _function(item);
            _dictionary.AddOrUpdate(key, item, (x, y) => y);
        }

        public TItem Wait(TKey key)
        {
            TItem item;
            int count = 0;
            while (!_dictionary.TryGetValue(key, out item) && count < 100)
            {
                Thread.Sleep(50);
                count++;
            }
            return item;
        }

        public TItem GetLast()
        {
            return _last;
        }
    }
}
