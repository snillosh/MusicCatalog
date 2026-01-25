using MusicCatalog.Domain.Tracks;

namespace MusicCatalog.Application.Tracks;

public interface ITrackRepository
{
    Task<IReadOnlyList<Track>> GetByAlbumIdAsync(Guid albumId, CancellationToken ct);
    Task AddAsync(Track track, CancellationToken ct);
    Task<bool> ExistsTrackNumberAsync(Guid albumId, int trackNumber, CancellationToken ct);
}
