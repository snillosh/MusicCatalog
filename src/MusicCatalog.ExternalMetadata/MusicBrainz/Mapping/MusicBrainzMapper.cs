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

    public static AlbumImportPreview ToAlbumImportPreview(
        this IRelease release)
    {
        var artistName = release.ArtistCredit.FirstOrDefault()?.Name ?? "Unknown Artist";
        var releaseYear = release.Date?.Year.ToString();

        var tracks = release.Media
            .SelectMany(x => x.Tracks)
            .Select(x =>
            {
                var trackNumber = x.Position ?? 0;
                var duration = x.Length.GetValueOrDefault().TotalSeconds;

                return new TrackPreview(x.Title, trackNumber, (int)duration);
            })
            .ToList();

        return new AlbumImportPreview(
        release.Id,
        release.Title,
        artistName,
        releaseYear,
        tracks);
    }
}
