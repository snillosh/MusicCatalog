using MusicCatalog.Contracts.Albums;
using MusicCatalog.Contracts.Artists;
using MusicCatalog.Contracts.Common.Paging;
using MusicCatalog.Contracts.Tracks;

namespace MusicCatalog.ApiClient;

public interface IMusicCatalogApiClient
{
    Task<PagedResult<AlbumListItemDto>> GetAlbumsAsync(
        int page = 1,
        int pageSize = 50,
        int? releasedAfter = null,
        CancellationToken cancellationToken = default);

    Task<AlbumDto> CreateAlbumAsync(Guid artistId, CreateAlbumRequest request,
        CancellationToken ct = default);

    Task<TrackDto> CreateTrackAsync(Guid albumId, CreateTrackRequest request,
        CancellationToken ct = default);

    Task<IReadOnlyList<ArtistDto>> GetArtistsAsync(CancellationToken cancellationToken = default);

    Task<ArtistDto> CreateArtistAsync(CreateArtistRequest request, CancellationToken ct = default);
}
