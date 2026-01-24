using MusicCatalog.Domain.Artists;

namespace MusicCatalog.Application.Artists;

public interface IArtistRepository
{
    Task<IReadOnlyList<Artist>> GetAllAsync(CancellationToken ct);
    Task<Artist?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Artist?> GetByIdTrackedAsync(Guid id, CancellationToken ct);
    Task AddAsync(Artist artist, CancellationToken ct);
    Task<bool> ExistsByNameAsync(string name, CancellationToken ct);
    Task UpdateAsync(Artist artist, CancellationToken ct);
    Task DeleteAsync(Artist artist, CancellationToken ct);
    Task<bool> ExistsByNameAsync(string name, Guid? excludeId, CancellationToken ct);
}
