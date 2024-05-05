namespace DynamicAuth.Infrastructures.Security;

/// <summary>
/// 政策來源
/// </summary>
public class PermissionAuthorizationProvider : IPermissionAuthorizationProvider
{
    private readonly AuthContext _authContext;
    public PermissionAuthorizationProvider(AuthContext authContext)
    {
        _authContext = authContext;
    }

    public async Task<IEnumerable<Role_Endpoint>> GetAuthorizationPolicy(string userId)
    {
        var roles = await _authContext.User_Role
            .Where(x => x.UserId == userId)
            .Select(x => x.RoleId)
            .ToListAsync();

        var roleEndpoints = await _authContext.Role_Endpoint
            .Where(x => roles.Contains(x.RoleId))
            .ToListAsync();

        return roleEndpoints;
    }
}
