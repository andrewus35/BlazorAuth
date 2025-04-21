using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace BlazorAuth.Components.Account;

internal static class IdentityComponentsEndpointRouteBuilderExtensions
{
    // These endpoints are required by the Identity Razor components defined in the /Components/Account/Pages directory of this project.
    public static IEndpointConventionBuilder MapAdditionalIdentityEndpoints(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);

        // Creating a route group
        var accountGroup = endpoints.MapGroup("/Account");

        // handles user logout
        accountGroup.MapPost("/Logout", async (
            HttpContext context, // Getting HttpContext directly
            [FromForm] string returnUrl) =>
        {
            // Using standard SignOutAsync to remove cookies
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return TypedResults.LocalRedirect($"~/{returnUrl}");
        });
        return accountGroup;
    }
    
}
