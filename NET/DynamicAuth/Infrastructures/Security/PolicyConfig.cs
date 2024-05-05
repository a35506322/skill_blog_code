namespace DynamicAuth.Infrastructures.Security;

public static class PolicyConfig
{
    public static void AddPolicy(this IServiceCollection services)
    {
        //services.AddAuthorization(options =>
        //{
        //    options.AddPolicy("Policy-Permission", policy =>
        //        policy.Requirements.Add(new PermissionAuthorizationRequirement()));
        //});
        services.AddSingleton<PermissionCache>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
        services.AddSingleton<IPermissionAuthorizationProvider, PermissionAuthorizationProvider>();
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationMiddlewareResultHandler, PermissionAuthorizationMiddlewareResultHandler>();
    }

}
