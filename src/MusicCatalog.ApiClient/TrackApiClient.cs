using System.Net.Http.Headers;
using System.Net.Http.Json;
using MusicCatalog.Contracts.Tracks;

namespace MusicCatalog.ApiClient;

public class TrackApiClient(IHttpClientFactory httpClientFactory, IAccessTokenStore tokenStore) : ITrackApiClient
{
    private readonly HttpClient _http = httpClientFactory.CreateClient("MusicCatalogApi");

    public async Task<TrackDto> CreateTrackAsync(
        Guid albumId,
        CreateTrackRequest request,
        CancellationToken ct = default)
    {
        var token = await tokenStore.GetAccessTokenAsync();

        if (!string.IsNullOrWhiteSpace(token))
        {
            _http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        var response = await _http.PostAsJsonAsync(
        $"api/albums/{albumId}/tracks",
        request,
        ct);

        if (!response.IsSuccessStatusCode)
        {
            var problem = await response.Content.ReadFromJsonAsync<ApiProblemDetails>(
            ct);

            throw new HttpRequestException(
            problem?.Detail ?? $"Failed to create track. Status code: {response.StatusCode}");
        }

        var track = await response.Content.ReadFromJsonAsync<TrackDto>(
        ct);

        return track ?? throw new InvalidOperationException("API returned an empty track response.");
    }
}
