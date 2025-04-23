using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DeltaCore.CacheHelper
{
    public class RedisCacheService 
    {
        private readonly IDistributedCache? _cach;
        public RedisCacheService(IDistributedCache? cach)
        {
            _cach = cach;
        }

        public async Task<T?> GetDataAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            if (_cach == null)
                return default;

            var data = await _cach.GetStringAsync(key, cancellationToken);
            if (string.IsNullOrEmpty(data))
                return default;

            return JsonSerializer.Deserialize<T>(data);
        }

        public async Task<string> SetDataAsync<T>(string key, T data, CancellationToken cancellationToken = default)
        {
            try
            {
                if (_cach == null)
                    return "Cache service is unavailable";

                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };

                var serializedData = JsonSerializer.Serialize(data);
                await _cach.SetStringAsync(key, serializedData, options, cancellationToken);

                return "ok";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        public async Task<string> SetDataAsync<T>(string key, T data, DistributedCacheEntryOptions options, CancellationToken cancellationToken = default)
        {
            try
            {
                if (_cach == null)
                    return "Cache service is unavailable";

                string serializedData = JsonSerializer.Serialize(data);
                await _cach.SetStringAsync(key, serializedData, options, cancellationToken);
                return "ok";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        public async Task<string> DelDataAsync(string key, CancellationToken cancellationToken = default)
        {
            try
            {
                if (_cach == null)
                    return "Cache service is unavailable";

                await _cach.RemoveAsync(key, cancellationToken);
                return "deleted";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
    }
}
