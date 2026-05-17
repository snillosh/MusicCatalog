using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using MusicCatalog.ApiClient;

namespace MusicCatalog.Web.Auth;

public sealed class ServerAccessTokenStore(ProtectedSessionStorage sessionStorage) : IAccessTokenStore
{
    private const string AccessTokenKey = "access_token";

    public async Task<string?> GetAccessTokenAsync()
    {
        var result = await sessionStorage.GetAsync<string>(AccessTokenKey);
        return result.Success ? result.Value : null;
    }

    public async Task SetAccessTokenAsync(string token) => await sessionStorage.SetAsync(AccessTokenKey, token);

    public async Task ClearAsync() => await sessionStorage.DeleteAsync(AccessTokenKey);
}
