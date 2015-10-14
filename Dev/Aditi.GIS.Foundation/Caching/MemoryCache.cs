using System;
using System.Runtime.Caching;

namespace Aditi.GIS.Foundation.Caching
{
    public class InMemoryCache : ICacheObject
    {
        private static readonly Lazy<InMemoryCache> _Instance = new Lazy<InMemoryCache>(() => new InMemoryCache());
        private volatile static MemoryCache cacheInstance = MemoryCache.Default;

        public static DateTimeOffset OneDay
        {
            get
            {
                return DateTimeOffset.Now.AddDays(1);
            }
        }

        public static DateTimeOffset OneWeek
        {
            get
            {
                return DateTimeOffset.Now.AddDays(7);
            }
        }

        private InMemoryCache()
        {
        }

        public static InMemoryCache Instance
        {
            get
            {
                return _Instance.Value;
            }
        }

        public object this[string key]
        {
            get
            {
                return this.Get<object>(key);
            }

            set
            {
                this.Set<object>(key, value);
            }
        }

        public void Set<T>(string key, T value)
        {
            cacheInstance.Set(key, value, ObjectCache.InfiniteAbsoluteExpiration);
        }

        public void Set<T>(string key, T value, DateTimeOffset timeout)
        {
            cacheInstance.Set(key, value, new CacheItemPolicy() { AbsoluteExpiration = timeout });
        }

        public void Set<T>(string key, T value, string regionName)
        {
            cacheInstance.Set(key, value, ObjectCache.InfiniteAbsoluteExpiration, regionName);
        }

        public void Set<T>(string key, T value, DateTimeOffset timeout, string regionName)
        {
            cacheInstance.Set(key, value, new CacheItemPolicy() { AbsoluteExpiration = timeout }, regionName);
        }

        public T Get<T>(string key)
        {
            object value = cacheInstance.Get(key);
            if (value == null)
            {
                return default(T);
            }

            return (T)value;
        }

        public T Get<T>(string key, string regionName)
        {
            object value = cacheInstance.Get(key, regionName);
            if (value == null)
            {
                return default(T);
            }

            return (T)value;
        }

        public bool TryGet<T>(string key, out T result)
        {
            object value = cacheInstance.Get(key);
            if (value == null)
            {
                result = default(T);
                return false;
            }

            result = (T)value;
            return true;
        }

        public bool TryGet<T>(string key, string regionName, out T result)
        {
            object value = cacheInstance.Get(key);
            if (value == null)
            {
                result = default(T);
                return false;
            }

            result = (T)value;
            return true;
        }
    }
}
