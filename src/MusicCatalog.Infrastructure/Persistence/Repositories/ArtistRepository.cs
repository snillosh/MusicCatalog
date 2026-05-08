using Microsoft.EntityFrameworkCore;
using MusicCatalog.Application.Artists;
using MusicCatalog.Contracts.Artists;
using MusicCatalog.Contracts.Common.Paging;
using MusicCatalog.Domain.Artists;

namespace MusicCatalog.Infrastructure.Persistence.Repositories;

public class ArtistRepository(MusicCatalogDbContext db) : IArtistRepository
{
    public async Task<PagedResult<ArtistDto>> GetAllAsync(int page, int pageSize, CancellationToken ct)
    {
        var query = db.Artists.AsNoTracking();

        var totalCount = await query.CountAsync(ct);

        query = query
            .OrderBy(a => a.Name)
            .ThenBy(a => a.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        var items = await query
            .Select(a => new ArtistDto(
            a.Id,
            a.Name,
            a.Country))
            .ToListAsync(ct);

        return new PagedResult<ArtistDto>(items, page, pageSize, totalCount);
    }

    public async Task<Artist?> GetByIdAsync(Guid id, CancellationToken ct) =>
        await db.Artists.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id, ct);

    public async Task<Artist?> GetByIdTrackedAsync(Guid id, CancellationToken ct)
        => await db.Artists.FirstOrDefaultAsync(a => a.Id == id, ct);

    public async Task AddAsync(Artist artist, CancellationToken ct)
    {
        db.Artists.Add(artist);
        await db.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken ct) =>
        await db.Artists.AnyAsync(a => a.Name.ToLower() == name.ToLower(), ct);

    public async Task UpdateAsync(Artist artist, CancellationToken ct) => await db.SaveChangesAsync(ct);

    public async Task DeleteAsync(Artist artist, CancellationToken ct)
    {
        db.Artists.Remove(artist);
        await db.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsByNameAsync(string name, Guid? excludeId, CancellationToken ct)
    {
        var query = db.Artists.AsQueryable();

        if (excludeId is not null)
        {
            query = query.Where(a => a.Id != excludeId.Value);
        }

        var lowered = name.ToLower();
        return await query.AnyAsync(a => a.Name.ToLower() == lowered, ct);
    }
}
