namespace MusicCatalog.ApiClient;

public interface IAccessTokenStore
{
    Task<string?> GetAccessTokenAsync();
    Task SetAccessTokenAsync(string token);
    Task ClearAsync();
}
