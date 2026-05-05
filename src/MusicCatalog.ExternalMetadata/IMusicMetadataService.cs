using MusicCatalog.Contracts.Albums;

namespace MusicCatalog.ExternalMetadata;

public interface IMusicMetadataService
{
    public Task<IReadOnlyList<AlbumSearchResult>> FindSimpleAlbumsAsync(string query);

    public Task<AlbumImportPreview> LookupAlbumImportPreviewAsync(Guid releaseId);
}
