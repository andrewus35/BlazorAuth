using BlazorAuth.Components;
using BlazorAuth.Components.Account;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents() // adding services for Razor components
    .AddInteractiveServerComponents(); // enabling interactive server rendering

builder.Services.AddCascadingAuthenticationState(); // Registers a service that allows components to inherit authentication state from parent components.
// for working with Identity
builder.Services.AddScoped<IdentityUserAccessor>(); // provides access to the current user
builder.Services.AddScoped<IdentityRedirectManager>(); // manages redirects during authorization
builder.Services.AddScoped<AuthenticationStateProvider, CustomRevalidatingAuthenticationStateProvider>(); // authentication state provider that verifies and updates information about the current user.

// Configures authentication in the application:
builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme; // Sets default authentication schemes
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme; // Adds support for cookie authentication with Identity
    })
    .AddIdentityCookies();


builder.Services.AddSingleton<IUserStore<ApplicationUser>, CustomUserStore>();
builder.Services.AddSingleton<IUserPasswordStore<ApplicationUser>>(sp =>
    (IUserPasswordStore<ApplicationUser>)sp.GetRequiredService<IUserStore<ApplicationUser>>());
builder.Services.AddSingleton<IUserSecurityStampStore<ApplicationUser>>(sp =>
    (IUserSecurityStampStore<ApplicationUser>)sp.GetRequiredService<IUserStore<ApplicationUser>>());

builder.Services.AddIdentityCore<ApplicationUser>(options => // Configures Identity: Requires account confirmation before login
{
    options.SignIn.RequireConfirmedAccount = true;
    // Simplifying password requirements for local development
    options.Password.RequireDigit = false;              // whether password requires digits
    options.Password.RequireLowercase = false;          // whether password requires lowercase letters
    options.Password.RequireUppercase = false;          // whether password requires uppercase letters
    options.Password.RequireNonAlphanumeric = false;    // whether password requires special characters
    options.Password.RequiredLength = 6;                // minimum password length
}) 
    .AddSignInManager()             // Adds sign-in manager
    .AddDefaultTokenProviders();    // Adds token providers for password recovery, etc.


var app = builder.Build();

if (!app.Environment.IsDevelopment()) // checking if not in development mode
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true); // adding error handling in production mode
    app.UseHsts(); // adding HSTS (HTTP Strict Transport Security) for improved security
}

app.UseHttpsRedirection(); // redirecting HTTP to HTTPS


app.UseAntiforgery(); // Adds protection against CSRF attacks.

app.MapStaticAssets();                  // Configures routing for static files (JS, CSS, images).
app.MapRazorComponents<App>()           // Configures routing for Razor components, specifying App as the root component.
    .AddInteractiveServerRenderMode();  // enabling interactive rendering mode

app.MapAdditionalIdentityEndpoints();   // Adds additional endpoints needed for Identity components.

app.Run();
