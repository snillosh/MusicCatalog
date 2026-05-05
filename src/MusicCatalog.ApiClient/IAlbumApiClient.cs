using MusicCatalog.Contracts.Albums;
using MusicCatalog.Contracts.Common.Paging;

namespace MusicCatalog.ApiClient;

public interface IAlbumApiClient
{
    Task<PagedResult<AlbumListItemDto>> GetAlbumsAsync(
        int page = 1,
        int pageSize = 50,
        int? releasedAfter = null,
        CancellationToken cancellationToken = default);
    Task<AlbumDto> CreateAlbumAsync(Guid artistId, CreateAlbumRequest request,
        CancellationToken ct = default);
}
