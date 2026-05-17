namespace MusicCatalog.Application.Authentication;

public sealed record UserLoginResult(
    string UserId,
    string Email,
    IReadOnlyCollection<string> Roles);
