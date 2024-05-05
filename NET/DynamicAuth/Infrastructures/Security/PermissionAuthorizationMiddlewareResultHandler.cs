using Microsoft.AspNetCore.Authorization.Policy;
using System.Text.Json;

namespace DynamicAuth.Infrastructures.Security;

/// <summary>
/// 這邊會覆蓋掉原本的 AuthorizationMiddlewareResultHandler 例如401、403的處理
/// </summary>
public class PermissionAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    private readonly ILogger<PermissionAuthorizationMiddlewareResultHandler> _logger;

    public PermissionAuthorizationMiddlewareResultHandler(
        ILogger<PermissionAuthorizationMiddlewareResultHandler> logger)
    {
        this._logger = logger;
    }

    public async Task HandleAsync(
        RequestDelegate next,
        HttpContext context,
        AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        // token 驗證失敗
        if (authorizeResult.Challenged)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(ResultResponseFactory.TokenFail());
            return;
        }

        // 授權失敗
        var permissionAuthorizationRequirements = policy.Requirements.OfType<PermissionAuthorizationRequirement>();

        if (authorizeResult.Forbidden
            && permissionAuthorizationRequirements.Any())
        {
            context.Response.StatusCode = 403;
            string errMsg = JsonSerializer.Serialize(authorizeResult.AuthorizationFailure.FailureReasons);
            this._logger.LogInformation("{AuthorizationFailureResults}",
                ResultResponseFactory.PolicFail(errMsg));

            await context.Response.WriteAsJsonAsync(ResultResponseFactory.PolicFail("連絡相關人員"));
            return;
        }

        await next(context);
    }
}
