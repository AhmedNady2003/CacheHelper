# CacheHelper
# DeltaCore.CacheHelper

`DeltaCore.CacheHelper` is a lightweight .NET library designed to simplify caching operations in your .NET applications using either **in-memory** or **distributed (Redis)** cache mechanisms.

This library abstracts common caching patterns to help developers interact with caching in a clean, reusable, and testable manner.

---

## Features

- **In-Memory Caching** using `IMemoryCache`
- **Distributed Caching** using `IDistributedCache` (supports Redis)
- Generic methods for `Get`, `Set`, and `Delete`
- Overloaded `Set` methods to support custom expiration
- Fully asynchronous for distributed cache
- Supports `SlidingExpiration`, `AbsoluteExpiration`, and customizable options

---

## Installation

Once published on NuGet, you’ll be able to install it using:

```bash
dotnet add package DeltaCore.CacheHelper
```
---
## 🚀 Usage

### 1. MemoryCacheService (In-Memory)
```csharp
public class SomeService
{
    private readonly MemoryCachService _cache;

    public SomeService(MemoryCachService cache)
    {
        _cache = cache;
    }

    public void SetItem()
    {
        _cache.SetData("user:1", new { Name = "Ahmed", Age = 21 });
    }

    public object? GetItem()
    {
        return _cache.GetData<object>("user:1");
    }

    public void RemoveItem()
    {
        _cache.DelData("user:1");
    }
}
```
You can also pass custom expiration:
```csharp
var options = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(15));
_cache.SetData("key", value, options);
```

### 2. RedisCacheService (Distributed)
```csharp
public class SomeService
{
    private readonly RedisCacheService _cache;

    public SomeService(RedisCacheService cache)
    {
        _cache = cache;
    }

    public async Task SetAsync()
    {
        await _cache.SetDataAsync("product:1", new { Id = 1, Name = "Bread" });
    }

    public async Task<object?> GetAsync()
    {
        return await _cache.GetDataAsync<object>("product:1");
    }

    public async Task DeleteAsync()
    {
        await _cache.DelDataAsync("product:1");
    }
}
```
With custom expiration:
```csharp
var options = new DistributedCacheEntryOptions
{
    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
};
await _cache.SetDataAsync("key", value, options);
```
---
## Configuration
Add Services in `Program.cs`
```csharp
builder.Services.AddMemoryCache();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});
builder.Services.AddScoped<MemoryCachService>();
builder.Services.AddScoped<RedisCacheService>();
```
---
## Project Structure
```pash
DeltaCore.CacheHelper/
├── MemoryCacheService.cs     # Handles in-memory caching
├── RedisCacheService.cs      # Handles distributed (Redis) caching
├── DeltaCore.CacheHelper.csproj
```
---
## GitHub Actions (CI/CD)
This project includes a GitHub Action to automatically publish the package to NuGet when you push a tag in the format `v*.*.*.`

Workflow file:`.github/workflows/nuget-publish.yml`
 
---
## License
This project is licensed under the MIT License.

---
## Author
Created by Ahmed Nady

GitHub: `AhmedNady2003`