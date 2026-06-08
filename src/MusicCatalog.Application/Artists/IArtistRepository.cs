using MusicCatalog.Contracts.Artists;
using MusicCatalog.Contracts.Common.Paging;
using MusicCatalog.Domain.Artists;

namespace MusicCatalog.Application.Artists;

public interface IArtistRepository
{
    Task<PagedResult<ArtistDto>> GetAllAsync(int page, int pageSize, CancellationToken ct);
    Task<Artist?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Artist?> GetByIdTrackedAsync(Guid id, CancellationToken ct);
    Task<Artist?> GetByNameTrackedAsync(string artistName, CancellationToken ct);
    Task AddAndSaveAsync(Artist artist, CancellationToken ct);
    void Add(Artist artist);
    Task<bool> ExistsByNameAsync(string name, CancellationToken ct);
    Task UpdateAsync(Artist artist, CancellationToken ct);
    Task DeleteAsync(Artist artist, CancellationToken ct);
    Task<bool> ExistsByNameAsync(string name, Guid? excludeId, CancellationToken ct);
}
