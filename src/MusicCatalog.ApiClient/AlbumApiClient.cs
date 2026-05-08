using System.Net.Http.Json;
using MusicCatalog.Contracts.Albums;
using MusicCatalog.Contracts.Common.Paging;

namespace MusicCatalog.ApiClient;

public class AlbumApiClient(IHttpClientFactory httpClientFactory) : IAlbumApiClient
{
    private readonly HttpClient _http = httpClientFactory.CreateClient("MusicCatalogApi");

    public async Task<PagedResult<AlbumListItemDto>> GetAlbumsAsync(
        int page = 1,
        int pageSize = 50,
        int? releasedAfter = null,
        CancellationToken cancellationToken = default)
    {
        var url = $"api/albums?page={page}&pageSize={pageSize}";

        if (releasedAfter.HasValue)
        {
            url += $"&releasedAfter={releasedAfter.Value}";
        }

        var result = await _http.GetFromJsonAsync<PagedResult<AlbumListItemDto>>(url, cancellationToken);

        return result ?? new PagedResult<AlbumListItemDto>([], 0, 0, 0);
    }

    public async Task<AlbumDto> CreateAlbumAsync(
        Guid artistId,
        CreateAlbumRequest request,
        CancellationToken ct = default)
    {
        var response = await _http.PostAsJsonAsync(
        $"api/artists/{artistId}/albums",
        request,
        ct);

        if (!response.IsSuccessStatusCode)
        {
            var problem = await response.Content.ReadFromJsonAsync<ApiProblemDetails>(
            ct);

            throw new HttpRequestException(
            problem?.Detail ?? $"Failed to create album. Status code: {response.StatusCode}");
        }

        var album = await response.Content.ReadFromJsonAsync<AlbumDto>(
        ct);

        return album ?? throw new InvalidOperationException("API returned an empty album response.");
    }
}
