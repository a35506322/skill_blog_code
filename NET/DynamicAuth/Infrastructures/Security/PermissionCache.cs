using Microsoft.Extensions.Caching.Memory;

namespace DynamicAuth.Infrastructures.Security;

public class PermissionCache
{
    public MemoryCache Cache { get; } = new MemoryCache(
      new MemoryCacheOptions
      {
          SizeLimit = 2
      });
}
