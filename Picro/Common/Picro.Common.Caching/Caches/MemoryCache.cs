using System;
using Microsoft.Extensions.Caching.Memory;

namespace Picro.Common.Caching.Caches
{
    public class MemoryCache<TKey, TValue>
    {
        private readonly IMemoryCache _underlyingMemoryCache;

        public MemoryCache()
        {
            _underlyingMemoryCache = new MemoryCache(new MemoryCacheOptions());
        }

        public void PutItem(TKey key, TValue value, TimeSpan? timeToLive = null)
        {
            if (timeToLive == null)
            {
                _underlyingMemoryCache.Set(key, value);
            }
            else
            {
                _underlyingMemoryCache.Set(key, value, timeToLive.Value);
            }
        }

        public TValue GetItem(TKey key)
        {
            return _underlyingMemoryCache.Get<TValue>(key);
        }

        public void Remove(TKey key)
        {
            _underlyingMemoryCache.Remove(key);
        }

        public bool HasValue(TKey key)
        {
            return _underlyingMemoryCache.TryGetValue(key, out _);
        }
    }
}