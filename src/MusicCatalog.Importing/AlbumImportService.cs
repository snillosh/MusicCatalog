using MusicCatalog.ApiClient;
using MusicCatalog.Contracts.Albums;
using MusicCatalog.Contracts.Artists;
using MusicCatalog.Contracts.Tracks;

namespace MusicCatalog.Importing;

public sealed class AlbumImportService(IMusicCatalogApiClient apiClient) : IAlbumImportService
{

    public async Task ImportAlbumAsync(
        AlbumImportPreview selectedAlbum,
        CancellationToken cancellationToken = default)
    {
        var artists = await apiClient.GetArtistsAsync(cancellationToken);

        var artist = artists.FirstOrDefault(x =>
            string.Equals(x.Name, selectedAlbum.ArtistName, StringComparison.OrdinalIgnoreCase));

        artist ??= await apiClient.CreateArtistAsync(
        new CreateArtistRequest(selectedAlbum.ArtistName, "WW"),
        cancellationToken);

        var releaseYear = int.TryParse(selectedAlbum.ReleaseDate, out var year)
            ? year
            : (int?)null;

        var album = await apiClient.CreateAlbumAsync(
        artist.Id,
        new CreateAlbumRequest(selectedAlbum.Title, releaseYear),
        cancellationToken);

        foreach (var track in selectedAlbum.Tracks)
        {
            await apiClient.CreateTrackAsync(
            album.Id,
            new CreateTrackRequest(
            track.TrackNumber,
            track.Title,
            track.DurationSeconds),
            cancellationToken);
        }
    }
}
