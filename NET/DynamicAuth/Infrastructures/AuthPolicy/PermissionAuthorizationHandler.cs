namespace DynamicAuth.Infrastructures.AuthPolicy;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionAuthorizationRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        PermissionAuthorizationRequirement requirement)
    {
        Console.WriteLine($"2. PermissionAuthorizationHandler 處理需求: {requirement}");
        
        Console.WriteLine("IsAuthenticated token 未過");
        if (context.User.Identity.IsAuthenticated == false)
        {
            return;
        }
        // ....處理流程
        context.Fail(new AuthorizationFailureReason(this,
            $"Invalid Permission"));

        if (context.HasFailed == false)
        {
            context.Succeed(requirement);
        }
    }
}
