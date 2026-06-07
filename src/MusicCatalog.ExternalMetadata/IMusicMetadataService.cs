using MusicCatalog.Contracts.Albums;

namespace MusicCatalog.ExternalMetadata;

public interface IMusicMetadataService
{
    public Task<IReadOnlyList<AlbumGroupSearchResult>> FindSimpleAlbumGroupsAsync(
        string query,
        CancellationToken ct = default);

    public Task<AlbumImportPreview?> LookupAlbumImportPreviewAsync(Guid releaseGroupId, CancellationToken ct = default);
}
