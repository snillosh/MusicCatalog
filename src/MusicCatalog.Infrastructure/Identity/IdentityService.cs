using Microsoft.AspNetCore.Identity;
using MusicCatalog.Application.Authentication;

namespace MusicCatalog.Infrastructure.Identity;

public sealed class IdentityService(
    UserManager<ApplicationUser> userManager)
    : IIdentityService
{
    public async Task<UserLoginResult?> ValidateCredentialsAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user is null)
        {
            return null;
        }

        var validPassword = await userManager.CheckPasswordAsync(user, password);

        if (!validPassword)
        {
            return null;
        }

        var roles = await userManager.GetRolesAsync(user);

        return new UserLoginResult(
            user.Id,
            user.Email!,
            roles.AsReadOnly());
    }
    
    public async Task<bool> CreateUserAsync(
        string email,
        string password,
        string? displayName,
        CancellationToken cancellationToken = default)
    {
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            DisplayName = displayName ?? string.Empty
        };

        var result = await userManager.CreateAsync(user, password);

        return result.Succeeded;
    }
}
