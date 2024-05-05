using Microsoft.AspNetCore.Authorization.Policy;

namespace DynamicAuth.Infrastructures.AuthPolicy;

public class PermissionAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    public async Task HandleAsync(RequestDelegate next,
        HttpContext context,
        AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        // 3. 處理結果當Policy 政策檢驗到失敗時
        await next(context);
    }
}
