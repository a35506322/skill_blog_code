using Microsoft.AspNetCore.Authorization.Policy;
using System.Text.Json;

namespace DynamicAuth.Infrastructures.Security;

/// <summary>
/// 這邊會覆蓋掉原本的 AuthorizationMiddlewareResultHandler 例如401、403的處理
/// </summary>
public class PermissionAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    public async Task HandleAsync(
        RequestDelegate next,
        HttpContext context,
        AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        Console.WriteLine($"PermissionAuthorizationMiddlewareResultHandler => 依據驗證與授權回應");
        // token 驗證失敗
        if (authorizeResult.Challenged)
        {
            Console.WriteLine($"PermissionAuthorizationMiddlewareResultHandler =>Token 驗證失敗 回傳401");
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(ResultResponseFactory.TokenFail());
            return;
        }

        var permissionAuthorizationRequirements = policy.Requirements.OfType<PermissionAuthorizationRequirement>();

        // 授權失敗
        if (authorizeResult.Forbidden
            && permissionAuthorizationRequirements.Any())
        {
            context.Response.StatusCode = 403;
            string errMsg = JsonSerializer.Serialize(authorizeResult.AuthorizationFailure.FailureReasons);
            Console.WriteLine($"PermissionAuthorizationMiddlewareResultHandler =>授權失敗 回傳403 '{errMsg}'");
            await context.Response.WriteAsJsonAsync(ResultResponseFactory.PolicFail("連絡相關人員"));
            return;
        }

        Console.WriteLine($"PermissionAuthorizationMiddlewareResultHandler => 授權成功");
        await next(context);
    }
}
