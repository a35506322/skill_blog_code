namespace DynamicAuth.Infrastructures.Security;

public interface IPermissionAuthorizationProvider
{
    /// <summary>
    /// 取得使用者的權限政策
    /// </summary>
    /// <param name="userId">token name get</param>
    /// <returns></returns>
    Task<IEnumerable<Role_Endpoint>> GetAuthorizationPolicy(string userId);
}
