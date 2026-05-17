namespace MusicCatalog.Contracts.Authentication;

public sealed record RegisterRequest(
    string Email,
    string Password,
    string? DisplayName);
