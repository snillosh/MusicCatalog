using FluentAssertions;
using MusicCatalog.Application.Artists;
using MusicCatalog.Application.Artists.ListArtists;
using MusicCatalog.Contracts.Artists;
using MusicCatalog.Contracts.Common.Paging;
using NSubstitute;
using NUnit.Framework;

namespace MusicCatalog.Application.Tests.Artists;

[TestFixture]
public class ListArtistsHandlerTests
{
    private readonly IArtistRepository _repository = Substitute.For<IArtistRepository>();
    private readonly ListArtistsHandler _handler;

    public ListArtistsHandlerTests()
    {
        _handler = new ListArtistsHandler(_repository);
    }

    [Test]
    public async Task Handle_ReturnsPagedArtists()
    {
        var request = new ListArtistsQuery(1, 10);

        var expected = new PagedResult<ArtistDto>(
        [
            new ArtistDto(Guid.NewGuid(), "Magdalena Bay", "US"),
            new ArtistDto(Guid.NewGuid(), "Cocteau Twins", "UK")
        ],
        2,
        1,
        10);

        _repository.GetAllAsync(request.Page, request.PageSize, Arg.Any<CancellationToken>())
            .Returns(expected);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().BeSameAs(expected);

        await _repository.Received(1)
            .GetAllAsync(request.Page, request.PageSize, Arg.Any<CancellationToken>());
    }
}
