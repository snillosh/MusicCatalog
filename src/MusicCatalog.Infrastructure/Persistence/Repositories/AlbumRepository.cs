using Microsoft.EntityFrameworkCore;
using MusicCatalog.Application.Albums;
using MusicCatalog.Contracts.Albums;
using MusicCatalog.Contracts.Common.Paging;
using MusicCatalog.Domain.Albums;

namespace MusicCatalog.Infrastructure.Persistence.Repositories;

public sealed class AlbumRepository(MusicCatalogDbContext db) : IAlbumRepository
{
    public async Task<IReadOnlyList<Album>> GetByArtistIdAsync(Guid artistId, CancellationToken ct)
        => await db.Albums.AsNoTracking()
            .Where(a => a.ArtistId == artistId)
            .OrderBy(a => a.ReleaseYear)
            .ThenBy(a => a.Title)
            .ToListAsync(ct);

    public async Task DeleteAsync(Album album, CancellationToken ct)
    {
        db.Albums.Remove(album);
        await db.SaveChangesAsync(ct);
    }

    public async Task<Album?> GetByIdAsync(Guid id, CancellationToken ct) =>
        await db.Albums.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id, ct);

    public async Task AddAsync(Album album, CancellationToken ct)
    {
        db.Albums.Add(album);
        await db.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsWithTitleAsync(Guid artistId, string title, CancellationToken ct)
    {
        var lowered = title.Trim().ToLower();
        return await db.Albums.AnyAsync(a =>
            a.ArtistId == artistId && a.Title.ToLower() == lowered, ct);
    }

    public async Task<IReadOnlyList<Album>> GetAllAsync(CancellationToken ct) =>
        await db.Albums.AsNoTracking().OrderBy(a => a.ArtistId).ThenBy(a => a.Title).ToListAsync(ct);

    public async Task<PagedResult<AlbumListItemDto>> GetAllWithArtistNameAsync(int page, int pageSize,
        int? releasedAfter,
        CancellationToken ct)
    {
        var query = db.Albums.AsNoTracking();

        if (releasedAfter is not null)
            query = query.Where(a => a.ReleaseYear != null && a.ReleaseYear >= releasedAfter);

        var totalCount = await query.CountAsync(ct);

        query = query
            .OrderBy(a => a.ArtistId)
            .ThenBy(a => a.Title)
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        var items = await query
            .Select(a => new AlbumListItemDto(
            a.Id,
            a.ArtistId,
            a.Artist.Name,
            a.Title,
            a.ReleaseYear))
            .ToListAsync(ct);

        return new PagedResult<AlbumListItemDto>(items, page, pageSize, totalCount);
    }

    public async Task<Album?> GetByIdTrackedAsync(Guid id, CancellationToken ct) =>
        await db.Albums.FirstOrDefaultAsync(a => a.Id == id, ct);

    public async Task UpdateAsync(Album album, CancellationToken ct) => await db.SaveChangesAsync(ct);

    public async Task<bool> ExistsByNameAsync(string name, Guid? excludeId, CancellationToken ct)
    {
        var query = db.Albums.AsQueryable();

        if (excludeId is not null)
            query = query.Where(a => a.Id != excludeId.Value);

        var lowered = name.ToLower();
        return await query.AnyAsync(a => a.Title.ToLower() == lowered, ct);
    }
}
