using Microsoft.Extensions.Caching.Memory;

namespace DynamicAuth.Infrastructures.Security;

/// <summary>
/// 政策來源
/// </summary>
public class PermissionAuthorizationProvider : IPermissionAuthorizationProvider
{
    private readonly IServiceProvider _serviceProvider;
    private readonly PermissionCache _permissionCache;
    private const string userRoleCacheKey = "User_RoleCache";
    private const string roleEndpointCacheKey = "Role_EndpointCache";
    public PermissionAuthorizationProvider(PermissionCache permissionCache, IServiceProvider serviceProvider)
    {
        _permissionCache = permissionCache;
        _serviceProvider = serviceProvider;
    }

    public async Task<IEnumerable<Role_Endpoint>> GetAuthorizationPolicy(string userId)
    {
        Console.WriteLine($"PermissionAuthorizationProvider => GetAuthorizationPolicy  UserId: '{userId}'");
        if (!_permissionCache.Cache.TryGetValue(userRoleCacheKey, out List<User_Role> cacheResultForUserRoles))
        {
            using var scope = _serviceProvider.CreateScope();
            var _authContext = scope.ServiceProvider.GetRequiredService<AuthContext>();

            // cacheKey不存在於快取,重取資料
            cacheResultForUserRoles = await _authContext.User_Role.ToListAsync();

            // 設定快取的使用量和到期時間
            var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSize(1)
            .SetSlidingExpiration(TimeSpan.FromHours(1));

            // 將資料和快取設定加到快取裡面
            _permissionCache.Cache.Set(userRoleCacheKey, cacheResultForUserRoles, cacheEntryOptions);
        }

        if (!_permissionCache.Cache.TryGetValue(roleEndpointCacheKey, out List<Role_Endpoint> cacheResultForRoleEndpoints))
        {
            using var scope = _serviceProvider.CreateScope();
            var _authContext = scope.ServiceProvider.GetRequiredService<AuthContext>();
            // cacheKey不存在於快取,重取資料
            cacheResultForRoleEndpoints = await _authContext.Role_Endpoint.ToListAsync();

            // 設定快取的使用量和到期時間
            var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSize(1)
            .SetSlidingExpiration(TimeSpan.FromHours(1));

            // 將資料和快取設定加到快取裡面
            _permissionCache.Cache.Set(roleEndpointCacheKey, cacheResultForRoleEndpoints, cacheEntryOptions);
        }


        var roles = cacheResultForUserRoles
            .Where(x => x.UserId == userId)
            .Select(x => x.RoleId);

        var roleEndpoints = cacheResultForRoleEndpoints
            .Where(x => roles.Contains(x.RoleId));

        return roleEndpoints;
    }
}
