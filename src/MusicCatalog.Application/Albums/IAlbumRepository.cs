using MusicCatalog.Application.Albums.Dto;
using MusicCatalog.Domain.Albums;

namespace MusicCatalog.Application.Albums;

public interface IAlbumRepository
{
    Task<IReadOnlyList<Album>> GetByArtistIdAsync(Guid artistId, CancellationToken ct);
    Task AddAsync(Album album, CancellationToken ct);
    Task<bool> ExistsWithTitleAsync(Guid artistId, string title, CancellationToken ct);
    Task<IReadOnlyList<Album>> GetAllAsync(CancellationToken ct);
    Task<IReadOnlyList<AlbumListItemDto>> GetAllWithArtistNameAsync(CancellationToken ct);
}
