using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
namespace DeltaCore.CacheHelper
{
    public class MemoryCacheService
    {
        private readonly IMemoryCache _memoryCache;
        public MemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;

        }
        public void DelData(string key)
        {
            _memoryCache.Remove(key);
        }

        public T? GetData<T>(string key)
        {
            return _memoryCache.TryGetValue(key, out var value) ? (T)value! : default;
        }


        public void SetData<T>(string key, T data)
        {
            if (string.IsNullOrWhiteSpace(key) || data == null)
                throw new ArgumentException("Invalid input for caching user data.");
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(10));
            _memoryCache.Set(key, data, cacheEntryOptions);
        }

        public void SetData<T>(string key, T data, MemoryCacheEntryOptions option)
        {
            if (string.IsNullOrWhiteSpace(key) || data == null)
                throw new ArgumentException("Invalid input for caching user data.");
            _memoryCache.Set(key, data, option);
        }
    }
}
