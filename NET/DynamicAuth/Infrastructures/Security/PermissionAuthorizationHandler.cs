using Microsoft.AspNetCore.Mvc.Controllers;

namespace DynamicAuth.Infrastructures.Security;

/// <summary>
/// 處利政策邏輯，可以隨著PolicyName去做更改
/// </summary>
public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
{
    private IPermissionAuthorizationProvider _permissionAuthorizationProvider;
    public PermissionAuthorizationHandler(IPermissionAuthorizationProvider permissionAuthorizationProvider)
    {
        _permissionAuthorizationProvider = permissionAuthorizationProvider;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        PermissionAuthorizationRequirement requirement)
    {
        Console.WriteLine($"2. PermissionAuthorizationHandler 處理政策為: {requirement.PolicyName}");

        // 判斷是否有通過Token驗證
        if (context.User.Identity.IsAuthenticated == false)
        {
            context.Fail(new AuthorizationFailureReason(this, $"目前請求沒有通過Token驗證"));
            return;
        }

        // 取得 Token 中的 UserId
        var userId = context.User.Identity.Name;
        var permissions = await _permissionAuthorizationProvider.GetAuthorizationPolicy(userId);

        // 比對該User的EnpointId是否與ActionName相同
        // https://stackoverflow.com/questions/47809437/how-to-access-current-httpcontext-in-asp-net-core-2-custom-policy-based-authoriz
        if (context.Resource is HttpContext httpContext)
        {
            var endpoint = httpContext.GetEndpoint();
            var actionDescriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();

            var permission = permissions.SingleOrDefault(x => x.EndpointId == actionDescriptor.ActionName);
            if (permission is null)
            {
                context.Fail(new AuthorizationFailureReason(this, $"用戶 '{userId}'，沒有授權 '{requirement.PolicyName}'，沒有此'{actionDescriptor.ActionName}'權限"));
            }
        }

        if (context.HasFailed == false)
        {
            context.Succeed(requirement);
        }
    }
}
