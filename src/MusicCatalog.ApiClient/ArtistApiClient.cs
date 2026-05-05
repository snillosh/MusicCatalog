using System.Net.Http.Json;
using MusicCatalog.Contracts.Artists;
using MusicCatalog.Contracts.Common.Paging;

namespace MusicCatalog.ApiClient;

public class ArtistApiClient(IHttpClientFactory httpClientFactory) : IArtistApiClient
{
    private readonly HttpClient _http = httpClientFactory.CreateClient("MusicCatalogApi");

    public async Task<PagedResult<ArtistDto>> GetArtistsAsync(CancellationToken cancellationToken = default)
    {
        var url = "api/artists";

        var result = await _http.GetFromJsonAsync<PagedResult<ArtistDto>>(url, cancellationToken);

        return result ?? new PagedResult<ArtistDto>([],0,0,0);
    }

    public async Task<ArtistDto> CreateArtistAsync(CreateArtistRequest request, CancellationToken ct = default)
    {
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
