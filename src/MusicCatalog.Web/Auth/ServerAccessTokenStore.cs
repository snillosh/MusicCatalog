using MusicCatalog.ApiClient;

namespace MusicCatalog.Web.Auth;

public sealed class ServerAccessTokenStore : IAccessTokenStore
{
    private string? _accessToken;

    public Task<string?> GetAccessTokenAsync() =>
        Task.FromResult(_accessToken);

    public Task SetAccessTokenAsync(string token)
    {
        _accessToken = token;
        return Task.CompletedTask;
    }

    public Task ClearAsync()
    {
        _accessToken = null;
        return Task.CompletedTask;
    }
}
