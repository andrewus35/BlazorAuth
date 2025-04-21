using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace BlazorAuth.Components.Account;

public class CustomRevalidatingAuthenticationStateProvider : RevalidatingServerAuthenticationStateProvider
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IOptions<IdentityOptions> _options;

    public CustomRevalidatingAuthenticationStateProvider(
        ILoggerFactory loggerFactory,
        IServiceScopeFactory scopeFactory,
        IOptions<IdentityOptions> options)
        : base(loggerFactory)
    {
        _scopeFactory = scopeFactory;
        _options = options;
    }

    protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(30);

    protected override async Task<bool> ValidateAuthenticationStateAsync(
        AuthenticationState authenticationState, CancellationToken cancellationToken)
    {
        // Get the user manager from a new scope to ensure it fetches fresh data
        await using var scope = _scopeFactory.CreateAsyncScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        return await ValidateSecurityStampAsync(userManager, authenticationState.User);
    }

    private async Task<bool> ValidateSecurityStampAsync(UserManager<ApplicationUser> userManager, ClaimsPrincipal principal)
    {
    // 1. Get user information from the principal (ClaimsPrincipal)
    var userId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
    if (string.IsNullOrEmpty(userId))
    {
        return false;
    }

    // 2. Load the user from the store by ID
    var user = await userManager.FindByIdAsync(userId);
    if (user == null)
    {
        return false;
    }

    // 3. If UserManager doesn't support SecurityStamp, consider the session valid
    if (!userManager.SupportsUserSecurityStamp)
    {
        return true;
    }

    // 4. Get the SecurityStamp from the principal (stored in cookies)
    var securityStampClaimType = _options.Value.ClaimsIdentity.SecurityStampClaimType;
    var principalStamp = principal.FindFirstValue(securityStampClaimType);
    if (string.IsNullOrEmpty(principalStamp))
    {
        return false;
    }

    // 5. Get the current SecurityStamp from the user store
    var userStamp = await userManager.GetSecurityStampAsync(user);
    
    // 6. Compare the SecurityStamp from cookies with the SecurityStamp from the store
    return principalStamp == userStamp;
}
}