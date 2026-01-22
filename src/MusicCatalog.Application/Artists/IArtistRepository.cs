using MusicCatalog.Domain.Artists;

namespace MusicCatalog.Application.Artists;

public interface IArtistRepository
{
    Task<IReadOnlyList<Artist>> GetAllAsync(CancellationToken ct);
    Task<Artist?> GetByIdAsync(Guid id, CancellationToken ct);
    Task AddAsync(Artist artist, CancellationToken ct);
    Task<bool> ExistsByNameAsync(string name, CancellationToken ct);
}
