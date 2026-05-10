using MetaBrainz.MusicBrainz.Interfaces.Entities;
using MetaBrainz.MusicBrainz.Interfaces.Searches;
using MusicCatalog.Contracts.Albums;
using MusicCatalog.Contracts.Tracks;

namespace MusicCatalog.ExternalMetadata.MusicBrainz.Mapping;

internal static class MusicBrainzMapper
{
    public static IReadOnlyList<AlbumSearchResult> ToAlbumSearchResults(
        this ISearchResults<ISearchResult<IReleaseGroup>> releaseGroups)
    {
        return releaseGroups.Results
            .Select(x => new AlbumSearchResult(
            x.Item.Id,
            x.Item.Releases?.FirstOrDefault()?.Id ?? Guid.Empty,
            x.Item.Title,
            x.Item.ArtistCredit.FirstOrDefault()?.Name ?? "Unknown Artist",
            x.Item.FirstReleaseDate?.Year.ToString(),
            10))
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
