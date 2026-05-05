using System.Net.Http.Json;
using MusicCatalog.Contracts.Albums;
using MusicCatalog.Contracts.Artists;
using MusicCatalog.Contracts.Common.Paging;
using MusicCatalog.Contracts.Tracks;

namespace MusicCatalog.ApiClient;

public sealed class MusicCatalogApiClient(HttpClient httpClient) : IMusicCatalogApiClient
{
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

        var result = await httpClient.GetFromJsonAsync<PagedResult<AlbumListItemDto>>(url, cancellationToken);

        return result ?? new PagedResult<AlbumListItemDto>([], 0, 0, 0);
    }

    public async Task<AlbumDto> CreateAlbumAsync(Guid artistId, CreateAlbumRequest request,
        CancellationToken ct = default)
    {
        var response = await httpClient.PostAsJsonAsync(
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

    public async Task<TrackDto> CreateTrackAsync(Guid albumId, CreateTrackRequest request,
        CancellationToken ct = default)
    {
        var response = await httpClient.PostAsJsonAsync(
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

    public async Task<IReadOnlyList<ArtistDto>> GetArtistsAsync(CancellationToken cancellationToken = default)
    {
        var url = "api/artists";

        var result = await httpClient.GetFromJsonAsync<IReadOnlyList<ArtistDto>>(url, cancellationToken);

        return result ?? new List<ArtistDto>();
    }

    public async Task<ArtistDto> CreateArtistAsync(CreateArtistRequest request, CancellationToken ct = default)
    {
        var response = await httpClient.PostAsJsonAsync(
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
