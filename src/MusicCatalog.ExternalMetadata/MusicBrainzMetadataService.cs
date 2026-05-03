using MetaBrainz.MusicBrainz;
using MetaBrainz.MusicBrainz.Interfaces.Browses;
using MetaBrainz.MusicBrainz.Interfaces.Entities;
using MetaBrainz.MusicBrainz.Interfaces.Searches;

namespace MusicCatalog.ExternalMetadata;

public sealed class MusicBrainzMetadataService : IMusicMetadataService, IDisposable
{
    private readonly Query _query = new("MusicCatalogTest", "0.1", "mailto:bevansalter@proton.me");

    public void Dispose() => _query.Close();

    public Task<ISearchResults<ISearchResult<IReleaseGroup>>> FindReleaseGroupsAsync(string query) =>
        _query.FindReleaseGroupsAsync(query, simple: true);

    public Task<IRelease> LookupReleaseAsync(Guid releaseId) => _query.LookupReleaseAsync(releaseId);

    public Task<IBrowseResults<IRecording>> BrowseReleaseRecordingsAsync(Guid releaseId) =>
        _query.BrowseReleaseRecordingsAsync(releaseId);
}
