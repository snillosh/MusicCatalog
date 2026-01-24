using Microsoft.EntityFrameworkCore;
using MusicCatalog.Application.Artists;
using MusicCatalog.Domain.Artists;

namespace MusicCatalog.Infrastructure.Persistence.Repositories;

public class ArtistRepository(MusicCatalogDbContext db) : IArtistRepository
{
    public async Task<IReadOnlyList<Artist>> GetAllAsync(CancellationToken ct) =>
        await db.Artists.AsNoTracking().OrderBy(a => a.Name).ToListAsync(ct);

    public async Task<Artist?> GetByIdAsync(Guid id, CancellationToken ct) =>
        await db.Artists.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id, ct);

    public async Task AddAsync(Artist artist, CancellationToken ct)
    {
        db.Artists.Add(artist);
        await db.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken ct) =>
        await db.Artists.AnyAsync(a => a.Name.ToLower() == name.ToLower(), ct);
}
