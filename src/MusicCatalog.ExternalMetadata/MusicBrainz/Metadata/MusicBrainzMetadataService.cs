using MetaBrainz.MusicBrainz;
using MusicCatalog.Contracts.Albums;
using MusicCatalog.ExternalMetadata.MusicBrainz.Mapping;

namespace MusicCatalog.ExternalMetadata.MusicBrainz.Metadata;

public sealed class MusicBrainzMetadataService : IMusicMetadataService, IDisposable
{
    private readonly Query _query = new("MusicCatalogTest", "0.1", "mailto:bevansalter@proton.me");

    public void Dispose() => _query.Close();

    public async Task<IReadOnlyList<AlbumSearchResult>> FindSimpleAlbumsAsync(string query)
    {
        var releaseGroups = await _query.FindReleaseGroupsAsync(query, simple: true);

        return releaseGroups.ToAlbumSearchResults();
    }

    public async Task<AlbumImportPreview> LookupAlbumImportPreviewAsync(Guid releaseId)
    {
        var release = await _query.LookupReleaseAsync(
        releaseId,
        Include.Media
        | Include.Recordings
        | Include.ArtistCredits);

        return release.ToAlbumImportPreview();
    }
}
