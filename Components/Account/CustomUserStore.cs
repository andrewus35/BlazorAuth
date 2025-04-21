using Microsoft.AspNetCore.Identity;

namespace BlazorAuth.Components.Account;

public class CustomUserStore : IUserStore<ApplicationUser>, IUserPasswordStore<ApplicationUser>, IUserEmailStore<ApplicationUser>, IUserSecurityStampStore<ApplicationUser>
{
    private static readonly ApplicationUser AdminUser = new()
    {
        Id = "admin-id",
        UserName = "admin",
        NormalizedUserName = "ADMIN",
        Email = "admin",
        NormalizedEmail = "ADMIN",
        EmailConfirmed = true,
        SecurityStamp = Guid.NewGuid().ToString()
    };

    private static readonly Dictionary<string, string> UserPasswords = new()
    {
        { AdminUser.Id, PasswordHasherHelper.HashPassword("admin123") } // Hashed "admin123"
    };

    public Task<string> GetUserIdAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Id);
    }

    public Task<string?> GetUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.UserName);
    }

    public async Task SetUserNameAsync(ApplicationUser user, string? userName, CancellationToken cancellationToken)
    {
        user.UserName = userName;
        await Task.CompletedTask;
        return ;
    }

    public Task<string?> GetNormalizedUserNameAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.NormalizedUserName);
    }

    public Task SetNormalizedUserNameAsync(ApplicationUser user, string? normalizedName, CancellationToken cancellationToken)
    {
        user.NormalizedUserName = normalizedName;
        return Task.CompletedTask;
    }

    public Task<IdentityResult> CreateAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        // We don't allow creating new users in this simplified version
        return Task.FromResult(IdentityResult.Failed(new IdentityError { Description = "Creating new users is not supported" }));
    }

    public Task<IdentityResult> UpdateAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        // We don't allow updating users in this simplified version
        return Task.FromResult(IdentityResult.Success);
    }

    public Task<IdentityResult> DeleteAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        // We don't allow deleting users in this simplified version
        return Task.FromResult(IdentityResult.Failed(new IdentityError { Description = "Deleting users is not supported" }));
    }

    public async Task<ApplicationUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        if (userId == AdminUser.Id)
        {
            return AdminUser;
        }
        return null;
    }

    public async Task<ApplicationUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        if (normalizedUserName == "ADMIN")
        {
            return AdminUser;
        }
        return null;
    }

    public Task SetPasswordHashAsync(ApplicationUser user, string? passwordHash, CancellationToken cancellationToken)
    {
        // We don't allow changing passwords in this simplified version
        return Task.CompletedTask;
    }

    public async Task<string?> GetPasswordHashAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        if (user.Id == AdminUser.Id && UserPasswords.TryGetValue(user.Id, out var passwordHash))
        {
            return passwordHash;
        }
        return null;
    }

    public Task<bool> HasPasswordAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Id == AdminUser.Id && UserPasswords.ContainsKey(user.Id));
    }

    public async Task<ApplicationUser?> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        if (normalizedEmail == "ADMIN")
        {
            return AdminUser;
        }
        return null;
    }

    public Task<string?> GetEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.Email);
    }

    public Task<bool> GetEmailConfirmedAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.EmailConfirmed);
    }

    public Task<string?> GetNormalizedEmailAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.NormalizedEmail);
    }

    public Task SetEmailAsync(ApplicationUser user, string? email, CancellationToken cancellationToken)
    {
        user.Email = email;
        return Task.CompletedTask;
    }

    public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed, CancellationToken cancellationToken)
    {
        user.EmailConfirmed = confirmed;
        return Task.CompletedTask;
    }

    public Task SetNormalizedEmailAsync(ApplicationUser user, string? normalizedEmail, CancellationToken cancellationToken)
    {
        user.NormalizedEmail = normalizedEmail;
        return Task.CompletedTask;
    }

    public Task SetSecurityStampAsync(ApplicationUser user, string stamp, CancellationToken cancellationToken)
    {
        user.SecurityStamp = stamp;
        return Task.CompletedTask;
    }

    public Task<string?> GetSecurityStampAsync(ApplicationUser user, CancellationToken cancellationToken)
    {
        return Task.FromResult(user.SecurityStamp);
    }

    public void Dispose()
    {
        // Nothing to dispose
    }
}
