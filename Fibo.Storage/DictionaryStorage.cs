using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Fibo.Storage
{
    public class DictionaryStorage<TKey, TValue> : IStorage<TKey, TValue>
    {
        private readonly IDictionary<TKey, TValue> _storage = new ConcurrentDictionary<TKey, TValue>();

        public TValue GetValue(TKey key)
        {
            if (_storage.ContainsKey(key))
            {
                return _storage[key];
            }
            return default(TValue);
        }

        public bool SetValue(TKey key, TValue value)
        {
            _storage[key] = value;
            return true;
        }
    }
}
