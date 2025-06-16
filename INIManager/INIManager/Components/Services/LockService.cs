using INIManager.Components.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace INIManager.Components.Services;

public class LockService : ILockService
{
    private readonly IMemoryCache _cache;

    public LockService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public bool SetLock(int id)
    {
        _cache.Set(id, true);
        return true;
    }

    public bool IsLocked(int id)
    {
        return _cache.TryGetValue(id, out _);
    }

    public bool RemoveLock(int id)
    {
        _cache.Remove(id);
        return true;
    }
}