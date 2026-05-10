using FluentAssertions;
using MusicCatalog.Application.Albums;
using MusicCatalog.Application.Albums.ListAlbumsByArtist;
using MusicCatalog.Contracts.Albums;
using MusicCatalog.Contracts.Common.Paging;
using NSubstitute;
using NUnit.Framework;

namespace MusicCatalog.Application.Tests.Albums;

[TestFixture]
public class ListAlbumsByArtistHandlerTests
{
    private readonly IAlbumRepository _albumRepository = Substitute.For<IAlbumRepository>();
    private readonly ListAlbumsByArtistHandler _handler;

    public ListAlbumsByArtistHandlerTests()
    {
        _handler = new ListAlbumsByArtistHandler(_albumRepository);
    }

    [Test]
    public async Task Handle_CallsGetAllWithArtistNameAsync()
    {
        var request = new ListAlbumsByArtistQuery(Guid.NewGuid());

        var albums = new List<AlbumListItemDto>
        {
            new(Guid.NewGuid(), Guid.NewGuid(), "Magdalena Bay", "Imaginal Disk", 2024),
            new(Guid.NewGuid(), Guid.NewGuid(), "Magdalena Bay", "Mercurial World", 2022)
        };

        _albumRepository.GetByArtistIdAsync(
            Arg.Any<Guid>(),
            request.Page,
            request.PageSize,
            CancellationToken.None)
            .Returns(new PagedResult<AlbumListItemDto>(albums, request.Page, request.PageSize, 1));

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Items.Should().BeEquivalentTo(albums);

        await _albumRepository.Received(1)
            .GetByArtistIdAsync(Arg.Any<Guid>(), request.Page, request.PageSize, CancellationToken.None);
    }
}
