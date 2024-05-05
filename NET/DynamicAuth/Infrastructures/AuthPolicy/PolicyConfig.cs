namespace DynamicAuth.Infrastructures.AuthPolicy;

public static class PolicyConfig
{
    public static void AddPolicy(this IServiceCollection services)
    {
        //services.AddAuthorization(options =>
        //{
        //    options.AddPolicy("Permission", policy =>
        //        policy.Requirements.Add(new PermissionAuthorizationRequirement()));
        //});
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationMiddlewareResultHandler, PermissionAuthorizationMiddlewareResultHandler>();
    }

}
