using System.Runtime.Caching;

namespace InvertApi.Utils;
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

    public bool Add(string key, object value, TimeSpan? ts = null, bool isAbsolute = true)
    {
        ts ??= TimeSpan.FromMinutes(15);
        if (Exists(key))
            return false;

        CacheItemPolicy cacheItemPolicy;
        if (isAbsolute)
        {
            cacheItemPolicy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.Add(ts.Value) };
        }
        else
        {
            cacheItemPolicy = new CacheItemPolicy { SlidingExpiration = ts.Value };
        }

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