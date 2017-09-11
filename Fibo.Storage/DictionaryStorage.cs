using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Fibo.Storage
{
    public class DictionaryStorage<TKey, TValue> : IStorage<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> storage = new ConcurrentDictionary<TKey, TValue>();

        public TValue GetValue(TKey key)
        {
            if (storage.ContainsKey(key))
            {
                return storage[key];
            }
            else
            {
                return default(TValue);
            }
        }

        public bool SetValue(TKey key, TValue value)
        {
            storage[key] = value;
            return true;
        }
    }
}
