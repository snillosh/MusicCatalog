using MusicCatalog.Application.Common.Results;

namespace MusicCatalog.Application.Authentication;

public interface IIdentityService
{
    Task<UserLoginResult?> ValidateCredentialsAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default);

    Task<Result> CreateUserAsync(
        string email,
        string password,
        string? displayName,
        CancellationToken cancellationToken = default);
}
