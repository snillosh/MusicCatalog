using MetaBrainz.MusicBrainz.CoverArt;

namespace MusicCatalog.ExternalMetadata;

public sealed class MusicBrainzCoverArtService : ICoverArtService, IDisposable
{
    private readonly CoverArt _coverArt = new();
    public async Task<Stream> FetchFrontAsync(Guid albumId)
    {
        var coverArtImage = await _coverArt.FetchFrontAsync(albumId);

        return coverArtImage.Data;
    }

    public void Dispose() => _coverArt.Close();
}
