using Microsoft.EntityFrameworkCore;
using MusicCatalog.Application.Genres;
using MusicCatalog.Domain.Genre;

namespace MusicCatalog.Infrastructure.Persistence.Repositories;

public class GenreRepository(MusicCatalogDbContext db) : IGenreRepository
{
    public async Task<Genre?> GetByIdAsync(Guid id, CancellationToken ct) 
        => await db.Genres.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id, ct);

    public async Task<Genre?> GetByIdTrackedAsync(Guid id, CancellationToken ct) =>
        await db.Genres.FirstOrDefaultAsync(a => a.Id == id, ct);

    public async Task AddAsync(Genre genre, CancellationToken ct)
    {
        db.Genres.Add(genre);
        await db.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsWithTitleAsync(string title, Guid? excludeId, CancellationToken ct)
    {
        var query = db.Genres.AsQueryable();

        if (excludeId is not null)
            query = query.Where(a => a.Id != excludeId.Value);

        var lowered = title.ToLower();
        return await query.AnyAsync(a => a.Title.ToLower() == lowered, ct);
    }
}
