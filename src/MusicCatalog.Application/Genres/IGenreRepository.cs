using MusicCatalog.Domain.Albums;
using MusicCatalog.Domain.Genre;

namespace MusicCatalog.Application.Genres;

public interface IGenreRepository
{
    Task<Genre?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Genre?> GetByIdTrackedAsync(Guid id, CancellationToken ct);
    Task AddAsync(Genre genre, CancellationToken ct);
    Task<bool> ExistsWithTitleAsync(string title, Guid? excludeId, CancellationToken ct);
}
