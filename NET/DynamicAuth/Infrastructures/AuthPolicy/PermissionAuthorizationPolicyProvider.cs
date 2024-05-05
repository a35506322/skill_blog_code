using Microsoft.Extensions.Options;

namespace DynamicAuth.Infrastructures.AuthPolicy;

public class PermissionAuthorizationPolicyProvider : IAuthorizationPolicyProvider
{
    public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

    public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
    {
        FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();

    public Task<AuthorizationPolicy> GetFallbackPolicyAsync() => FallbackPolicyProvider.GetFallbackPolicyAsync();


    public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
    {
        Console.WriteLine($"1. PermissionAuthorizationPolicyProvider 註冊政策: {policyName}");
        if (policyName.StartsWith("Policy", StringComparison.OrdinalIgnoreCase))
        {
            var policy = new AuthorizationPolicyBuilder();
            policy.AddRequirements(new PermissionAuthorizationRequirement());
            return Task.FromResult(policy.Build());
        }


        return FallbackPolicyProvider.GetPolicyAsync(policyName);
    }
}
