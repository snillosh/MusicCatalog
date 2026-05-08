using MusicCatalog.Contracts.Tracks;

namespace MusicCatalog.ApiClient;

public interface ITrackApiClient
{
    Task<TrackDto> CreateTrackAsync(
        Guid albumId,
        CreateTrackRequest request,
        CancellationToken ct = default);
}
