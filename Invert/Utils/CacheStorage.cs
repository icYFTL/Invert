using System.Runtime.Caching;
using System;
using System.Linq;

namespace Invert.Utils;
public class CacheStorage
{
    private readonly MemoryCache _memoryCache;

    public CacheStorage()
    {
        _memoryCache = MemoryCache.Default;
    }

    public string? Get (string key)
    {
        return _memoryCache.FirstOrDefault(x => x.Key == key).Value.ToString();
    }

    public bool Exists(string key)
    {
        return _memoryCache.Contains(key);
    }

    public bool Add(string key, object value)
    {
        if (Exists(key))
            return false;

        var cacheItemPolicy = new CacheItemPolicy();

        _memoryCache.Add(key, value, cacheItemPolicy);

        return true;
    }

    public bool Remove(string key)
    {
        if (Exists(key))
        {
            _memoryCache.Remove(key);
            return true;
        }

        return false;
    }

    public bool Update(string key, object value)
    {
        if (!Exists(key))
            return false;

        Remove(key);
        Add(key, value);

        return true;
    }

}