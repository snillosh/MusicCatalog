using MetaBrainz.MusicBrainz;
using MusicCatalog.Contracts.Albums;
using MusicCatalog.ExternalMetadata.MusicBrainz.Mapping;

namespace MusicCatalog.ExternalMetadata.MusicBrainz.Metadata;

public sealed class MusicBrainzMetadataService : IMusicMetadataService, IDisposable
{
    private readonly Query _query = new("MusicCatalogTest", "0.1", "mailto:bevansalter@proton.me");

    public void Dispose() => _query.Close();

    public async Task<IReadOnlyList<AlbumGroupSearchResult>> FindSimpleAlbumGroupsAsync(
        string query,
        CancellationToken ct = default)
    {
        var releaseGroups = await _query.FindReleaseGroupsAsync(query, simple: true, cancellationToken: ct);

        return releaseGroups.ToAlbumGroupSearchResults();
    }

    public async Task<AlbumImportPreview?> LookupAlbumImportPreviewAsync(
        Guid releaseGroupId,
        CancellationToken ct = default)
    {
        var browseResults = await _query.BrowseReleaseGroupReleasesAsync(
        releaseGroupId,
        inc: Include.Media | Include.ArtistCredits | Include.Recordings);

        var releaseResults = browseResults.Results.GetMostSuitableRelease();

        return releaseResults.ToAlbumImportPreview();
    }
}
