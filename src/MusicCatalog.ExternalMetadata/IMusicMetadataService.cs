using MetaBrainz.MusicBrainz.Interfaces.Browses;
using MetaBrainz.MusicBrainz.Interfaces.Entities;
using MusicCatalog.Contracts.Albums;

namespace MusicCatalog.ExternalMetadata;

public interface IMusicMetadataService
{
    public Task<IReadOnlyList<AlbumSearchResult>> FindSimpleAlbumsAsync(string query);

    public Task<AlbumImportPreview> LookupAlbumImportPreviewAsync(Guid releaseId);

    public Task<IBrowseResults<IRecording>> BrowseReleaseRecordingsAsync(Guid releaseId);
}
