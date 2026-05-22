using MusicCatalog.Contracts.Tracks;

namespace MusicCatalog.ApiClient;

public interface ITrackApiClient
{
    Task<TrackDto> CreateTrackAsync(
        Guid albumId,
        CreateTrackRequest request,
        CancellationToken ct = default);

    Task<IReadOnlyList<TrackDto>> GetTracksByAlbumIdAsync(Guid albumId, CancellationToken ct = default);
}
