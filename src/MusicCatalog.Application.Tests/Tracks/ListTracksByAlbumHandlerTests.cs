using FluentAssertions;
using MusicCatalog.Application.Tracks;
using MusicCatalog.Application.Tracks.ListTracksByAlbum;
using MusicCatalog.Domain.Tracks;
using NSubstitute;
using NUnit.Framework;

namespace MusicCatalog.Application.Tests.Tracks;

[TestFixture]
public class ListTracksByAlbumHandlerTests
{
    private readonly ITrackRepository _repository = Substitute.For<ITrackRepository>();
    private readonly ListTracksByAlbumHandler _handler;

    public ListTracksByAlbumHandlerTests()
    {
        _handler = new ListTracksByAlbumHandler(_repository);
    }

    [Test]
    public async Task Handle_ReturnsTracksForAlbum()
    {
        var albumId = Guid.NewGuid();

        var tracks = new[]
        {
            new Track(albumId, 1, "She Looked Like Me!", 187),
            new Track(albumId, 2, "Killing Time", 225)
        };

        var request = new ListTracksByAlbumQuery(albumId);

        _repository.GetByAlbumIdAsync(albumId, Arg.Any<CancellationToken>())
            .Returns(tracks);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().HaveCount(2);

        result[0].Id.Should().Be(tracks[0].Id);
        result[0].AlbumId.Should().Be(albumId);
        result[0].TrackNumber.Should().Be(1);
        result[0].Title.Should().Be("She Looked Like Me!");
        result[0].DurationSeconds.Should().Be(187);

        result[1].Id.Should().Be(tracks[1].Id);
        result[1].AlbumId.Should().Be(albumId);
        result[1].TrackNumber.Should().Be(2);
        result[1].Title.Should().Be("Killing Time");
        result[1].DurationSeconds.Should().Be(225);

        await _repository.Received(1)
            .GetByAlbumIdAsync(albumId, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Handle_WhenAlbumHasNoTracks_ReturnsEmptyList()
    {
        var albumId = Guid.NewGuid();
        var request = new ListTracksByAlbumQuery(albumId);

        _repository.GetByAlbumIdAsync(albumId, Arg.Any<CancellationToken>())
            .Returns(Array.Empty<Track>());

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().BeEmpty();

        await _repository.Received(1)
            .GetByAlbumIdAsync(albumId, Arg.Any<CancellationToken>());
    }
}
