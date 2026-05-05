using MusicCatalog.ApiClient;
using MusicCatalog.Contracts.Albums;
using MusicCatalog.Contracts.Artists;
using MusicCatalog.Contracts.Tracks;

namespace MusicCatalog.Importing;

public sealed class AlbumImportService(
    IAlbumApiClient albumApiClient,
    IArtistApiClient artistApiClient,
    ITrackApiClient trackApiClient) : IAlbumImportService
{

    public async Task ImportAlbumAsync(
        AlbumImportPreview selectedAlbum,
        CancellationToken cancellationToken = default)
    {
        var artists = await artistApiClient.GetArtistsAsync(cancellationToken);

        var artist = artists.Items.FirstOrDefault(x =>
            string.Equals(x.Name, selectedAlbum.ArtistName, StringComparison.OrdinalIgnoreCase));

        artist ??= await artistApiClient.CreateArtistAsync(
        new CreateArtistRequest(selectedAlbum.ArtistName, "WW"),
        cancellationToken);

        var releaseYear = int.TryParse(selectedAlbum.ReleaseDate, out var year)
            ? year
            : (int?)null;

        var album = await albumApiClient.CreateAlbumAsync(
        artist.Id,
        new CreateAlbumRequest(selectedAlbum.Title, releaseYear),
        cancellationToken);

        foreach (var track in selectedAlbum.Tracks)
        {
            await trackApiClient.CreateTrackAsync(
            album.Id,
            new CreateTrackRequest(
            track.TrackNumber,
            track.Title,
            track.DurationSeconds),
            cancellationToken);
        }
    }
}
