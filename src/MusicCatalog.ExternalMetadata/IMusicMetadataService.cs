using MusicCatalog.Contracts.Albums;

namespace MusicCatalog.ExternalMetadata;

public interface IMusicMetadataService
{
    public Task<IReadOnlyList<AlbumGroupSearchResult>> FindSimpleAlbumGroupsAsync(string query);

    public Task<AlbumImportPreview> LookupAlbumImportPreviewAsync(Guid releaseGroupId);
}
