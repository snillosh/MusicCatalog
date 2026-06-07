using MusicCatalog.Application.Albums;
using MusicCatalog.Application.Artists;
using MusicCatalog.Application.Tracks;
using MusicCatalog.Contracts.Albums;
using MusicCatalog.Domain.Albums;
using MusicCatalog.Domain.Artists;
using MusicCatalog.Domain.Tracks;

namespace MusicCatalog.Application.Importing;

public class AlbumImportService(
    IArtistRepository artistRepo,
    IAlbumRepository albumRepo,
    ITrackRepository trackRepo) : IAlbumImportService
{
    public async Task<AlbumDto> ImportAlbumAsync(
        AlbumImportPreview selectedAlbum,
        CancellationToken cancellationToken = default)
    {
        var artist = await GetOrCreateArtistAsync(
        selectedAlbum.ArtistName,
        cancellationToken);

        var releaseYear = ParseReleaseYear(selectedAlbum.ReleaseDate);

        var newAlbum = new Album(artist.Id, selectedAlbum.Title, releaseYear);

        await albumRepo.AddAsync(
        newAlbum,
        cancellationToken);

        foreach (var track in selectedAlbum.Tracks)
        {
            await trackRepo.AddAsync(
            new Track(newAlbum.Id, track.TrackNumber, track.Title, track.DurationSeconds),
            cancellationToken);
        }

        return new AlbumDto(newAlbum.Id, artist.Id, newAlbum.Title, newAlbum.ReleaseYear);
    }

    private async Task<Artist> GetOrCreateArtistAsync(
        string artistName,
        CancellationToken cancellationToken)
    {
        var artist = await artistRepo.GetByNameTrackedAsync(
        artistName,
        cancellationToken);

        if (artist is not null)
        {
            return artist;
        }

        var newArtist = new Artist(artistName, "WW");

        await artistRepo.AddAsync(
        newArtist,
        cancellationToken);

        return newArtist;
    }

    private static int? ParseReleaseYear(string? releaseDate)
    {
        return int.TryParse(releaseDate, out var year)
            ? year
            : null;
    }
}
