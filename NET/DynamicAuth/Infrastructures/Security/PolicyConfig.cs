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
        services.AddScoped<IPermissionAuthorizationProvider, PermissionAuthorizationProvider>();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddSingleton<IAuthorizationMiddlewareResultHandler, PermissionAuthorizationMiddlewareResultHandler>();
    }

}
