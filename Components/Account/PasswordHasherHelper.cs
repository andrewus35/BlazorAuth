using Microsoft.AspNetCore.Identity;

namespace BlazorAuth.Components.Account;

public class PasswordHasherHelper
{
    // Use this method if you need to generate a password hash
    public static string HashPassword(string password)
    {
        var hasher = new PasswordHasher<ApplicationUser>();
        return hasher.HashPassword(new ApplicationUser(), password);
    }

    // This method can be used to verify the correctness of a password
    public static bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        var hasher = new PasswordHasher<ApplicationUser>();
        var result = hasher.VerifyHashedPassword(new ApplicationUser(), hashedPassword, providedPassword);
        return result == PasswordVerificationResult.Success;
    }
}