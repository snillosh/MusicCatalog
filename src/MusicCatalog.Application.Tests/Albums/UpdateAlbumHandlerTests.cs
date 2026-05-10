using FluentAssertions;
using MusicCatalog.Application.Albums;
using MusicCatalog.Application.Albums.UpdateAlbum;
using MusicCatalog.Contracts.Albums;
using MusicCatalog.Domain.Albums;
using NSubstitute;
using NUnit.Framework;

namespace MusicCatalog.Application.Tests.Albums;

[TestFixture]
public class UpdateAlbumHandlerTests
{

    [SetUp]
    public void SetUp()
    {
        _albumRepository = Substitute.For<IAlbumRepository>();
        _handler = new UpdateAlbumHandler(_albumRepository);
    }
    private IAlbumRepository _albumRepository = null!;
    private UpdateAlbumHandler _handler = null!;

    [Test]
    public async Task Handle_WithValidRequest_ReturnsAlbum()
    {
        var album = new Album(Guid.NewGuid(), "Imaginal Disk", 2024);

        var request = new UpdateAlbumCommand(album.Id, "NewTitle", 2025);

        var dto = new AlbumDto(album.Id, album.ArtistId, request.Title, request.ReleaseYear);

        _albumRepository.GetByIdTrackedAsync(request.Id, Arg.Any<CancellationToken>()).Returns(album);

        _albumRepository.ExistsByNameAsync(request.Title, request.Id, Arg.Any<CancellationToken>()).Returns(false);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(dto);

        await _albumRepository.Received(1).UpdateAsync(album, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Handle_WithUnknownAlbum_ReturnsNull()
    {
        var request = new UpdateAlbumCommand(Guid.NewGuid(), "NewTitle", 2025);

        _albumRepository.GetByIdTrackedAsync(request.Id, Arg.Any<CancellationToken>()).Returns((Album?)null);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().BeNull();

        await _albumRepository.DidNotReceive()
            .ExistsByNameAsync(Arg.Any<string>(), Arg.Any<Guid>(), Arg.Any<CancellationToken>());

        await _albumRepository.DidNotReceive()
            .UpdateAsync(Arg.Any<Album>(), Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Handle_WithDuplicateName_ReturnsNull()
    {
        var album = new Album(Guid.NewGuid(), "Imaginal Disk", 2024);

        var request = new UpdateAlbumCommand(Guid.NewGuid(), "NewTitle", 2025);

        _albumRepository.GetByIdTrackedAsync(request.Id, Arg.Any<CancellationToken>()).Returns(album);
        _albumRepository.ExistsByNameAsync(request.Title, request.Id, Arg.Any<CancellationToken>()).Returns(true);

        await FluentActions.Invoking(async () => await _handler.Handle(request, CancellationToken.None))
            .Should()
            .ThrowAsync<InvalidOperationException>();

        await _albumRepository.DidNotReceive()
            .UpdateAsync(Arg.Any<Album>(), Arg.Any<CancellationToken>());
    }
}
