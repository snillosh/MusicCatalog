using MusicCatalog.Domain.Tracks;

namespace MusicCatalog.Application.Tracks;

public interface ITrackRepository
{
    Task<IReadOnlyList<Track>> GetByAlbumIdAsync(Guid albumId, CancellationToken ct);
    Task AddAndSaveAsync(Track track, CancellationToken ct);
    void Add(Track track);
    Task<bool> ExistsTrackNumberAsync(Guid albumId, int trackNumber, CancellationToken ct);
}
