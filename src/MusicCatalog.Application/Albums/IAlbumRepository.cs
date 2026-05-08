using MusicCatalog.Contracts.Albums;
using MusicCatalog.Contracts.Common.Paging;
using MusicCatalog.Domain.Albums;

namespace MusicCatalog.Application.Albums;

public interface IAlbumRepository
{
    Task<PagedResult<AlbumListItemDto>> GetByArtistIdAsync(Guid artistId, int page, int pageSize, CancellationToken ct);
    Task<Album?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Album?> GetByIdTrackedAsync(Guid id, CancellationToken ct);
    Task AddAsync(Album album, CancellationToken ct);
    Task<bool> ExistsWithTitleAsync(Guid artistId, string title, CancellationToken ct);
    Task<IReadOnlyList<Album>> GetAllAsync(CancellationToken ct);

    Task DeleteAsync(Album album, CancellationToken ct);

    Task UpdateAsync(Album album, CancellationToken ct);

    Task<bool> ExistsByNameAsync(string name, Guid? excludeId, CancellationToken ct);

    Task<PagedResult<AlbumListItemDto>> GetAllWithArtistNameAsync(
        int page,
        int pageSize,
        int? releasedAfter,
        CancellationToken ct);
}
