using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using MyApp.Common.Constants;
using System.Threading.Tasks;

namespace MyApp.API.Authorization;

public class PermissionPolicyProvider : IAuthorizationPolicyProvider
{
    private DefaultAuthorizationPolicyProvider _fallbackPolicyProvider;
    
    public PermissionPolicyProvider(IOptions<AuthorizationOptions> options)
    {
        _fallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => _fallbackPolicyProvider.GetDefaultPolicyAsync();

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync() => _fallbackPolicyProvider.GetFallbackPolicyAsync();

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith("Permission:", StringComparison.OrdinalIgnoreCase))
        {
            var permissionString = policyName.Substring("Permission:".Length);
            
            var policy = new AuthorizationPolicyBuilder()
                .RequireClaim("permission", permissionString)
                .Build();
                
            return Task.FromResult<AuthorizationPolicy?>(policy);
        }
        
        return _fallbackPolicyProvider.GetPolicyAsync(policyName);
    }
}
