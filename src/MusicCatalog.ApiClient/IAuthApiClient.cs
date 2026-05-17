using MusicCatalog.Contracts.Authentication;

namespace MusicCatalog.ApiClient;

public interface IAuthApiClient
{
    Task<LoginResponse> LoginAsync(
        LoginRequest request,
        CancellationToken ct = default);

    Task RegisterAsync(
        RegisterRequest request,
        CancellationToken ct = default);
}
