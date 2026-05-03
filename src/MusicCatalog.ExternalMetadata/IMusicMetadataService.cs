using MetaBrainz.MusicBrainz.Interfaces.Browses;
using MetaBrainz.MusicBrainz.Interfaces.Entities;
using MetaBrainz.MusicBrainz.Interfaces.Searches;

namespace MusicCatalog.ExternalMetadata;

public interface IMusicMetadataService
{
    public Task<ISearchResults<ISearchResult<IReleaseGroup>>> FindReleaseGroupsAsync(string query);

    public Task<IRelease> LookupReleaseAsync(Guid releaseId);

    public Task<IBrowseResults<IRecording>> BrowseReleaseRecordingsAsync(Guid releaseId);
}
