namespace MusicCatalog.ExternalMetadata;

public interface ICoverArtService
{
    public Task<Stream> FetchFrontAsync(Guid albumId);
}
