using MusicCatalog.ApiClient;
using MusicCatalog.Contracts.Albums;
using MusicCatalog.Contracts.Artists;
using MusicCatalog.Contracts.Common.Paging;
using MusicCatalog.Contracts.Tracks;
using NSubstitute;
using NUnit.Framework;

namespace MusicCatalog.Importing.Tests;

[TestFixture]
public class AlbumImportServiceTests
{
    [SetUp]
    public void SetUp()
    {
        _albumApiClient = Substitute.For<IAlbumApiClient>();
        _artistApiClient = Substitute.For<IArtistApiClient>();
        _trackApiClient = Substitute.For<ITrackApiClient>();

        _service = new AlbumImportService(
        _albumApiClient,
        _artistApiClient,
        _trackApiClient);
    }
    private IAlbumApiClient _albumApiClient = null!;
    private IArtistApiClient _artistApiClient = null!;
    private ITrackApiClient _trackApiClient = null!;
    private AlbumImportService _service = null!;

    [Test]
    public async Task ImportAlbumAsync_WhenArtistExists_CreatesAlbumAndTracks()
    {
        var artist = new ArtistDto(Guid.NewGuid(), "Magdalena Bay", "US");
        var album = new AlbumDto(Guid.NewGuid(), artist.Id, "Imaginal Disk", 2024);

        var selectedAlbum = new AlbumImportPreview(
        Guid.NewGuid(),
        "Imaginal Disk",
        "magdalena bay",
        "2024",
        [
            new TrackPreview("She Looked Like Me!", 1, 187),
            new TrackPreview("Killing Time", 2, 225)
        ]);

        _artistApiClient.GetArtistsAsync()
            .Returns(
            new PagedResult<ArtistDto>(
            [artist],
            1,
            1,
            10));

        _albumApiClient.CreateAlbumAsync(
            artist.Id,
            Arg.Any<CreateAlbumRequest>(),
            Arg.Any<CancellationToken>())
            .Returns(album);

        await _service.ImportAlbumAsync(selectedAlbum, CancellationToken.None);

        await _artistApiClient.DidNotReceive()
            .CreateArtistAsync(Arg.Any<CreateArtistRequest>(), Arg.Any<CancellationToken>());

        await _albumApiClient.Received(1)
            .CreateAlbumAsync(
            artist.Id,
            Arg.Is<CreateAlbumRequest>(x =>
                x.Title == "Imaginal Disk" && x.ReleaseYear == 2024),
            Arg.Any<CancellationToken>());

        await _trackApiClient.Received(1)
            .CreateTrackAsync(
            album.Id,
            Arg.Is<CreateTrackRequest>(x =>
                x.TrackNumber == 1 && x.Title == "She Looked Like Me!" && x.DurationSeconds == 187),
            Arg.Any<CancellationToken>());

        await _trackApiClient.Received(1)
            .CreateTrackAsync(
            album.Id,
            Arg.Is<CreateTrackRequest>(x =>
                x.TrackNumber == 2 && x.Title == "Killing Time" && x.DurationSeconds == 225),
            Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task ImportAlbumAsync_WhenArtistDoesNotExist_CreatesArtist()
    {
        var createdArtist = new ArtistDto(Guid.NewGuid(), "Magdalena Bay", "WW");
        var album = new AlbumDto(Guid.NewGuid(), createdArtist.Id, "Imaginal Disk", 2024);

        var selectedAlbum = new AlbumImportPreview(
        Guid.NewGuid(),
        "Imaginal Disk",
        "Magdalena Bay",
        "2024",
        []);

        _artistApiClient.GetArtistsAsync()
            .Returns(
            new PagedResult<ArtistDto>(
            [],
            0,
            1,
            10));

        _artistApiClient.CreateArtistAsync(
            Arg.Any<CreateArtistRequest>(),
            Arg.Any<CancellationToken>())
            .Returns(createdArtist);

        _albumApiClient.CreateAlbumAsync(
            createdArtist.Id,
            Arg.Any<CreateAlbumRequest>(),
            Arg.Any<CancellationToken>())
            .Returns(album);

        await _service.ImportAlbumAsync(selectedAlbum, CancellationToken.None);

        await _artistApiClient.Received(1)
            .CreateArtistAsync(
            Arg.Is<CreateArtistRequest>(x =>
                x.Name == "Magdalena Bay" && x.Country == "WW"),
            Arg.Any<CancellationToken>());

        await _albumApiClient.Received(1)
            .CreateAlbumAsync(
            createdArtist.Id,
            Arg.Is<CreateAlbumRequest>(x =>
                x.Title == "Imaginal Disk" && x.ReleaseYear == 2024),
            Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task ImportAlbumAsync_WhenReleaseDateIsInvalid_CreatesAlbumWithNullReleaseYear()
    {
        var artist = new ArtistDto(Guid.NewGuid(), "Magdalena Bay", "US");
        var album = new AlbumDto(Guid.NewGuid(), artist.Id, "Imaginal Disk", null);

        var selectedAlbum = new AlbumImportPreview(
        Guid.NewGuid(),
        "Imaginal Disk",
        "Magdalena Bay",
        "Unknown",
        []);

        _artistApiClient.GetArtistsAsync()
            .Returns(
            new PagedResult<ArtistDto>(
            [artist],
            1,
            1,
            10));

        _albumApiClient.CreateAlbumAsync(
            artist.Id,
            Arg.Any<CreateAlbumRequest>(),
            Arg.Any<CancellationToken>())
            .Returns(album);

        await _service.ImportAlbumAsync(selectedAlbum, CancellationToken.None);

        await _albumApiClient.Received(1)
            .CreateAlbumAsync(
            artist.Id,
            Arg.Is<CreateAlbumRequest>(x =>
                x.Title == "Imaginal Disk" && x.ReleaseYear == null),
            Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task ImportAlbumAsync_WhenAlbumHasNoTracks_DoesNotCreateTracks()
    {
        var artist = new ArtistDto(Guid.NewGuid(), "Magdalena Bay", "US");
        var album = new AlbumDto(Guid.NewGuid(), artist.Id, "Imaginal Disk", 2024);

        var selectedAlbum = new AlbumImportPreview(
        Guid.NewGuid(),
        "Imaginal Disk",
        "Magdalena Bay",
        "2024",
        []);

        _artistApiClient.GetArtistsAsync()
            .Returns(
            new PagedResult<ArtistDto>(
            [artist],
            1,
            1,
            10));

        _albumApiClient.CreateAlbumAsync(
            artist.Id,
            Arg.Any<CreateAlbumRequest>(),
            Arg.Any<CancellationToken>())
            .Returns(album);

        await _service.ImportAlbumAsync(selectedAlbum, CancellationToken.None);

        await _trackApiClient.DidNotReceive()
            .CreateTrackAsync(
            Arg.Any<Guid>(),
            Arg.Any<CreateTrackRequest>(),
            Arg.Any<CancellationToken>());
    }
}
