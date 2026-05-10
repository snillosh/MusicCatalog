using FluentAssertions;
using MusicCatalog.Application.Albums;
using MusicCatalog.Application.Albums.GetAlbumById;
using MusicCatalog.Contracts.Albums;
using MusicCatalog.Domain.Albums;
using NSubstitute;
using NUnit.Framework;

namespace MusicCatalog.Application.Tests.Albums;

[TestFixture]
public class GetAlbumByIdHandlerTests
{
    private readonly IAlbumRepository _albumRepository = Substitute.For<IAlbumRepository>();
    private readonly GetAlbumByIdHandler _handler;

    public GetAlbumByIdHandlerTests()
    {
        _handler = new GetAlbumByIdHandler(_albumRepository);
    }

    [Test]
    public async Task Handle_WithValidId_ReturnsAlbum()
    {
        var album = new Album(Guid.NewGuid(), "Imaginal Disk", 2024);
        var dto = new AlbumDto(album.Id, album.ArtistId, album.Title, album.ReleaseYear);
        var request = new GetAlbumByIdQuery(album.Id);

        _albumRepository.GetByIdAsync(request.Id, CancellationToken.None).Returns(album);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(dto);
    }

    [Test]
    public async Task Handle_WithInvalidId_ReturnsNull()
    {
        var request = new GetAlbumByIdQuery(Guid.NewGuid());

        _albumRepository.GetByIdAsync(request.Id, CancellationToken.None).Returns((Album?)null);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().BeNull();
    }
}
