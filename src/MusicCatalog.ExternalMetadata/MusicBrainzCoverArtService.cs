using MetaBrainz.MusicBrainz.CoverArt;

namespace MusicCatalog.ExternalMetadata;

public sealed class MusicBrainzCoverArtService : ICoverArtService, IDisposable
{
    private readonly CoverArt _coverArt = new();
    public Task<CoverArtImage> FetchFrontAsync(Guid albumId) => _coverArt.FetchFrontAsync(albumId);

    public void Dispose() => _coverArt.Close();
}
