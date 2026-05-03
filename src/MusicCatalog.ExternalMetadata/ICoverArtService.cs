using MetaBrainz.MusicBrainz.CoverArt;

namespace MusicCatalog.ExternalMetadata;

public interface ICoverArtService
{
    public Task<CoverArtImage> FetchFrontAsync(Guid albumId);
}
