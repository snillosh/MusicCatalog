using FluentAssertions;
using MusicCatalog.Application.Albums;
using MusicCatalog.Application.Albums.ListAlbums;
using MusicCatalog.Contracts.Albums;
using MusicCatalog.Contracts.Common.Paging;
using NSubstitute;
using NUnit.Framework;

namespace MusicCatalog.Application.Tests.Albums;

[TestFixture]
public class ListAlbumsHandlerTests
{
    private readonly IAlbumRepository _albumRepository = Substitute.For<IAlbumRepository>();
    private readonly ListAlbumsHandler _handler;

    public ListAlbumsHandlerTests()
    {
        _handler = new ListAlbumsHandler(_albumRepository);
    }

    [Test]
    public async Task Handle_CallsAlbumRepository()
    {
        var request = new ListAlbumsQuery();

        var albums = new List<AlbumListItemDto>
        {
            new(Guid.NewGuid(), Guid.NewGuid(), "Magdalena Bay", "Imaginal Disk", 2024),
            new(Guid.NewGuid(), Guid.NewGuid(), "Magdalena Bay", "Mercurial World", 2022)
        };

        _albumRepository.GetAllWithArtistNameAsync(
            request.Page,
            request.PageSize,
            request.ReleasedAfter,
            CancellationToken.None)
            .Returns(new PagedResult<AlbumListItemDto>(albums, request.Page, request.PageSize, 1));

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Items.Should().BeEquivalentTo(albums);

        await _albumRepository.Received(1)
            .GetAllWithArtistNameAsync(request.Page, request.PageSize, request.ReleasedAfter, CancellationToken.None);
    }
}
