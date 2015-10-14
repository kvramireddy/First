using System;

namespace Aditi.GIS.Foundation.Caching
{
    public interface ICacheObject
    {
        object this[string key] { get; set; }

        T Get<T>(string key);
        T Get<T>(string key, string regionName);
        bool TryGet<T>(string key, out T result);
        bool TryGet<T>(string key, string regionName, out T result);
        void Set<T>(string key, T value);
        void Set<T>(string key, T value, string regionName);
        void Set<T>(string key, T value, DateTimeOffset timeout);
        void Set<T>(string key, T value, DateTimeOffset timeout, string regionName);
    }
}
