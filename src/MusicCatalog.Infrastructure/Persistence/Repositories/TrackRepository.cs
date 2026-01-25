using Microsoft.EntityFrameworkCore;
using MusicCatalog.Application.Tracks;
using MusicCatalog.Domain.Tracks;

namespace MusicCatalog.Infrastructure.Persistence.Repositories;

public sealed class TrackRepository(MusicCatalogDbContext db) : ITrackRepository
{
    public async Task<IReadOnlyList<Track>> GetByAlbumIdAsync(Guid albumId, CancellationToken ct) =>
        await db.Tracks.AsNoTracking()
            .Where(t => t.AlbumId == albumId)
            .OrderBy(t => t.TrackNumber)
            .ToListAsync(ct);

    public async Task AddAsync(Track track, CancellationToken ct)
    {
        db.Tracks.Add(track);
        await db.SaveChangesAsync(ct);
    }

    public Task<bool> ExistsTrackNumberAsync(Guid albumId, int trackNumber, CancellationToken ct) =>
        db.Tracks.AnyAsync(t => t.AlbumId == albumId && t.TrackNumber == trackNumber, ct);
}
