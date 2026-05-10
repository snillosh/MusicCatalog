using FluentAssertions;
using MusicCatalog.Application.Albums;
using MusicCatalog.Application.Albums.CreateAlbum;
using MusicCatalog.Application.Artists;
using MusicCatalog.Domain.Albums;
using MusicCatalog.Domain.Artists;
using NSubstitute;
using NUnit.Framework;

namespace MusicCatalog.Application.Tests.Albums;

[TestFixture]
public class CreateAlbumHandlerTests
{
    private readonly IArtistRepository _artistRepository = Substitute.For<IArtistRepository>();
    private readonly IAlbumRepository _albumRepository = Substitute.For<IAlbumRepository>();
    private readonly CreateAlbumHandler _handler;

    public CreateAlbumHandlerTests()
    {
        _handler = new CreateAlbumHandler(_artistRepository, _albumRepository);
    }

    [Test]
    public async Task Handle_WithValidAlbum_ReturnsSuccess()
    {
        var artist = new Artist("Magdalena Bay", "US");
        var request = new CreateAlbumCommand(artist.Id, "  Imaginal Disk  ", 2024);

        _artistRepository.GetByIdAsync(artist.Id, Arg.Any<CancellationToken>())
            .Returns(artist);

        _albumRepository.ExistsWithTitleAsync(artist.Id, "Imaginal Disk", Arg.Any<CancellationToken>())
            .Returns(false);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.ArtistId.Should().Be(artist.Id);
        result.Value.Title.Should().Be("Imaginal Disk");
        result.Value.ReleaseYear.Should().Be(2024);

        await _albumRepository.Received(1)
            .AddAsync(
            Arg.Is<Album>(a =>
                a.ArtistId == artist.Id && a.Title == "Imaginal Disk" && a.ReleaseYear == 2024),
            Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Handle_WhenArtistDoesNotExist_ReturnsFailure()
    {
        var artistId = Guid.NewGuid();

        var request = new CreateAlbumCommand(artistId, "  Imaginal Disk  ", 2024);

        _artistRepository.GetByIdAsync(artistId, Arg.Any<CancellationToken>())
            .Returns((Artist?)null);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();

        result.Error!.Code.Should().Be("artists.notFound");
        result.Error!.Message.Should().Be("Artist not found.");

        await _albumRepository.DidNotReceive()
            .ExistsWithTitleAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<CancellationToken>());

        await _albumRepository.DidNotReceive()
            .AddAsync(Arg.Any<Album>(), Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Handle_WithDuplicateTitle_ReturnsFailure()
    {
        var artist = new Artist("Magdalena Bay", "US");
        var request = new CreateAlbumCommand(artist.Id, "Imaginal Disk", 2024);

        _artistRepository.GetByIdAsync(artist.Id, Arg.Any<CancellationToken>())
            .Returns(artist);

        _albumRepository.ExistsWithTitleAsync(artist.Id, "Imaginal Disk", Arg.Any<CancellationToken>())
            .Returns(true);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();

        result.Error!.Code.Should().Be("albums.duplicate");
        result.Error!.Message.Should().Be("That artist already has an album with that title.");

        await _albumRepository.DidNotReceive()
            .AddAsync(Arg.Any<Album>(), Arg.Any<CancellationToken>());
    }
}
