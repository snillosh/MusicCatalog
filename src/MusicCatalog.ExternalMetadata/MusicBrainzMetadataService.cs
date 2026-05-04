using MetaBrainz.MusicBrainz;
using MetaBrainz.MusicBrainz.Interfaces.Browses;
using MetaBrainz.MusicBrainz.Interfaces.Entities;
using MusicCatalog.Contracts.Albums;
using MusicCatalog.Contracts.Tracks;

namespace MusicCatalog.ExternalMetadata;

public sealed class MusicBrainzMetadataService : IMusicMetadataService, IDisposable
{
    private readonly Query _query = new("MusicCatalogTest", "0.1", "mailto:bevansalter@proton.me");

    public void Dispose() => _query.Close();

    public Task<IBrowseResults<IRecording>> BrowseReleaseRecordingsAsync(Guid releaseId) =>
        _query.BrowseReleaseRecordingsAsync(releaseId, inc: Include.Media);

    public async Task<IReadOnlyList<AlbumSearchResult>> FindSimpleAlbumsAsync(string query)
    {
        var musicBrainzAlbums = await _query.FindReleaseGroupsAsync(query, simple: true);

        return musicBrainzAlbums.Results.Select(x =>
            new AlbumSearchResult(x.Item.Id, x.Item.Releases?.FirstOrDefault()?.Id ?? Guid.Empty,
            x.Item.Title,
            x.Item.ArtistCredit.FirstOrDefault().Name,
            x.Item.FirstReleaseDate?.Year.ToString(),
            10)).ToList();
    }

    public async Task<AlbumImportPreview> LookupAlbumImportPreviewAsync(Guid releaseId)
    {
        var musicBrainzRelease = await _query.LookupReleaseAsync(releaseId,
        Include.Media
        | Include.Recordings
        | Include.ArtistCredits);

        var trackList = musicBrainzRelease.Media.SelectMany(x => x.Tracks);

        var artistName = musicBrainzRelease.ArtistCredit.FirstOrDefault().Name;

        var releaseYear = musicBrainzRelease.Date?.Year.ToString();

        return new AlbumImportPreview(musicBrainzRelease.Id,
        musicBrainzRelease.Title,
        artistName,
        releaseYear,
        trackList.Select(x => {
            var trackNumber = x.Position ?? 0;
            var duration = x.Length.GetValueOrDefault().Seconds;
            return new TrackPreview(x.Title, trackNumber, duration);
        }).ToList());
    }
}
