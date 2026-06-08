using MusicCatalog.Domain.Genre;

namespace MusicCatalog.Application.Genres;

public interface IGenreRepository
{
    Task<Genre?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Genre?> GetByIdTrackedAsync(Guid id, CancellationToken ct);
    Task AddAndSaveAsync(Genre genre, CancellationToken ct);
    void Add(Genre genre);
    Task<bool> ExistsWithTitleAsync(string title, Guid? excludeId, CancellationToken ct);
}
