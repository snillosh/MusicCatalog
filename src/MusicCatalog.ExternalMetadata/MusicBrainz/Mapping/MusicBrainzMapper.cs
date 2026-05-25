using MetaBrainz.MusicBrainz.Interfaces.Entities;
using MetaBrainz.MusicBrainz.Interfaces.Searches;
using MusicCatalog.Contracts.Albums;
using MusicCatalog.Contracts.Tracks;

namespace MusicCatalog.ExternalMetadata.MusicBrainz.Mapping;

internal static class MusicBrainzMapper
{
    public static IReadOnlyList<AlbumGroupSearchResult> ToAlbumGroupSearchResults(
        this ISearchResults<ISearchResult<IReleaseGroup>> releaseGroups)
    {
        return releaseGroups.Results
            .Select(x => new AlbumGroupSearchResult(
            x.Item.Id,
            x.Item.Title,
            x.Item.ArtistCredit.FirstOrDefault()?.Name ?? "Unknown Artist",
            x.Item.FirstReleaseDate?.Year.ToString()))
            .ToList();
    }

    public static AlbumImportPreview ToAlbumImportPreview(this IRelease release)
    {
        var artistName = release.ArtistCredit.FirstOrDefault()?.Name ?? "Unknown Artist";
        var releaseYear = release.Date?.Year.ToString();
        var tracks = CreateTrackPreviews(release.Media);

        return new AlbumImportPreview(
        release.Id,
        release.Title,
        artistName,
        releaseYear,
        tracks);
    }

    private static List<TrackPreview> CreateTrackPreviews(IReadOnlyList<IMedium> media) => media.Count > 1
        ? CreateMultiDiscTrackPreviews(media)
        : CreateSingleDiscTrackPreviews(media[0]);

    private static List<TrackPreview> CreateMultiDiscTrackPreviews(IReadOnlyList<IMedium> media)
    {
        var tracks = media.SelectMany(x => x.Tracks).ToList();
        var previews = new List<TrackPreview>();

        for (var i = 0; i < tracks.Count; i++)
        {
            var track = tracks[i];

            previews.Add(
            new TrackPreview(
            track.Title,
            i + 1,
            track.GetDurationInSeconds()));
        }

        return previews;
    }

    private static List<TrackPreview> CreateSingleDiscTrackPreviews(IMedium? medium)
    {
        if (medium is null)
        {
            return [];
        }

        return medium.Tracks
            .Select(track => new TrackPreview(
            track.Title,
            track.Position ?? 0,
            track.GetDurationInSeconds()))
            .ToList();
    }

    private static int GetDurationInSeconds(this ITrack track) => (int)track.Length.GetValueOrDefault().TotalSeconds;
}
