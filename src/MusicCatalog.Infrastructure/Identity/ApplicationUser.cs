using Microsoft.AspNetCore.Identity;

namespace MusicCatalog.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    public string DisplayName { get; set; } = string.Empty;
}
