using FluentAssertions;
using MusicCatalog.Application.Albums;
using MusicCatalog.Application.Albums.DeleteAlbum;
using MusicCatalog.Domain.Albums;
using NSubstitute;
using NUnit.Framework;

namespace MusicCatalog.Application.Tests.Albums;

[TestFixture]
public class DeleteAlbumHandlerTests
{
    private readonly IAlbumRepository _albumRepository = Substitute.For<IAlbumRepository>();
    private readonly DeleteAlbumHandler _handler;

    public DeleteAlbumHandlerTests()
    {
        _handler = new DeleteAlbumHandler(_albumRepository);
    }

    [Test]
    public async Task Handle_WithValidAlbum_ReturnsSuccess()
    {
        var artistId = Guid.NewGuid();

        var album = new Album(artistId, "Imaginal Disk", 2024);

        var request = new DeleteAlbumCommand(album.Id);

        _albumRepository.GetByIdTrackedAsync(request.Id, CancellationToken.None).Returns(album);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().BeTrue();

        await _albumRepository.Received(1).DeleteAsync(album, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Handle_WithUnknownAlbum_ReturnsFalse()
    {
        var albumId = Guid.NewGuid();

        var request = new DeleteAlbumCommand(albumId);

        _albumRepository.GetByIdTrackedAsync(request.Id, CancellationToken.None).Returns((Album?)null);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().BeFalse();

        await _albumRepository.DidNotReceive().DeleteAsync(Arg.Any<Album>(), Arg.Any<CancellationToken>());
    }
}
