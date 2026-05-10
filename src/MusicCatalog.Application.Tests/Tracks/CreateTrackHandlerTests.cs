using FluentAssertions;
using MusicCatalog.Application.Albums;
using MusicCatalog.Application.Tracks;
using MusicCatalog.Application.Tracks.CreateTrack;
using MusicCatalog.Domain.Albums;
using MusicCatalog.Domain.Tracks;
using NSubstitute;
using NUnit.Framework;

namespace MusicCatalog.Application.Tests.Tracks;

[TestFixture]
public class CreateTrackHandlerTests
{
    private readonly IAlbumRepository _albumRepository = Substitute.For<IAlbumRepository>();
    private readonly ITrackRepository _trackRepository = Substitute.For<ITrackRepository>();
    private readonly CreateTrackHandler _handler;

    public CreateTrackHandlerTests()
    {
        _handler = new CreateTrackHandler(_albumRepository, _trackRepository);
    }

    [Test]
    public async Task Handle_WithValidTrack_ReturnsSuccess()
    {
        var artistId = Guid.NewGuid();
        var album = new Album(artistId, "Imaginal Disk", 2024);

        var request = new CreateTrackCommand(album.Id, 1, "  She Looked Like Me!  ", 187);

        _albumRepository.GetByIdAsync(album.Id, Arg.Any<CancellationToken>())
            .Returns(album);

        _trackRepository.ExistsTrackNumberAsync(album.Id, 1, Arg.Any<CancellationToken>())
            .Returns(false);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.AlbumId.Should().Be(album.Id);
        result.Value.TrackNumber.Should().Be(1);
        result.Value.Title.Should().Be("She Looked Like Me!");
        result.Value.DurationSeconds.Should().Be(187);

        await _trackRepository.Received(1)
            .AddAsync(
            Arg.Is<Track>(t =>
                t.AlbumId == album.Id
                && t.TrackNumber == 1
                && t.Title == "She Looked Like Me!"
                && t.DurationSeconds == 187),
            Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Handle_WhenAlbumDoesNotExist_ReturnsFailure()
    {
        var albumId = Guid.NewGuid();
        var request = new CreateTrackCommand(albumId, 1, "She Looked Like Me!", 187);

        _albumRepository.GetByIdAsync(albumId, Arg.Any<CancellationToken>())
            .Returns((Album?)null);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error!.Code.Should().Be("albums.notFound");
        result.Error.Message.Should().Be("Album not found.");

        await _trackRepository.DidNotReceive()
            .ExistsTrackNumberAsync(Arg.Any<Guid>(), Arg.Any<int>(), Arg.Any<CancellationToken>());

        await _trackRepository.DidNotReceive()
            .AddAsync(Arg.Any<Track>(), Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Handle_WithDuplicateTrackNumber_ReturnsFailure()
    {
        var artistId = Guid.NewGuid();
        var album = new Album(artistId, "Imaginal Disk", 2024);

        var request = new CreateTrackCommand(album.Id, 1, "  She Looked Like Me!  ", 187);

        _albumRepository.GetByIdAsync(album.Id, Arg.Any<CancellationToken>())
            .Returns(album);

        _trackRepository.ExistsTrackNumberAsync(album.Id, 1, Arg.Any<CancellationToken>())
            .Returns(true);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error!.Code.Should().Be("tracks.duplicateTrackNumber");
        result.Error.Message.Should().Be("That track number already exists for this album.");

        await _trackRepository.DidNotReceive()
            .AddAsync(Arg.Any<Track>(), Arg.Any<CancellationToken>());
    }
}
