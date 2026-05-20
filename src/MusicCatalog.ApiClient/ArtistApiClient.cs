using System.Net.Http.Headers;
using System.Net.Http.Json;
using MusicCatalog.Contracts.Artists;
using MusicCatalog.Contracts.Common.Paging;

namespace MusicCatalog.ApiClient;

public class ArtistApiClient(IHttpClientFactory httpClientFactory, IAccessTokenStore tokenStore) : IArtistApiClient
{
    private readonly HttpClient _http = httpClientFactory.CreateClient("MusicCatalogApi");

    public async Task<PagedResult<ArtistDto>> GetArtistsAsync(
        int page = 1,
        int pageSize = 50,
        CancellationToken cancellationToken = default)
    {
        var url = $"api/artists?page={page}&pageSize={pageSize}";

        var result = await _http.GetFromJsonAsync<PagedResult<ArtistDto>>(url, cancellationToken);

        return result ?? new PagedResult<ArtistDto>([], 0, 0, 0);
    }

    public async Task<ArtistDto> CreateArtistAsync(CreateArtistRequest request, CancellationToken ct = default)
    {
        var token = await tokenStore.GetAccessTokenAsync();

        if (!string.IsNullOrWhiteSpace(token))
        {
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await _http.PostAsJsonAsync(
        "api/artists",
        request,
        ct);

        if (!response.IsSuccessStatusCode)
        {
            var problem = await response.Content.ReadFromJsonAsync<ApiProblemDetails>(
            ct);

            throw new HttpRequestException(
            problem?.Detail ?? $"Failed to create artist. Status code: {response.StatusCode}");
        }

        var artist = await response.Content.ReadFromJsonAsync<ArtistDto>(
        ct);

        return artist ?? throw new InvalidOperationException("API returned an empty artist response.");
    }
}
