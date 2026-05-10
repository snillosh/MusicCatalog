using FluentAssertions;
using MusicCatalog.Application.Artists;
using MusicCatalog.Application.Artists.GetArtistById;
using MusicCatalog.Domain.Artists;
using NSubstitute;
using NUnit.Framework;

namespace MusicCatalog.Application.Tests.Artists;

[TestFixture]
public class GetArtistByIdHandlerTests
{
    private readonly IArtistRepository _repository = Substitute.For<IArtistRepository>();
    private readonly GetArtistByIdHandler _handler;

    public GetArtistByIdHandlerTests()
    {
        _handler = new GetArtistByIdHandler(_repository);
    }

    [Test]
    public async Task Handle_WhenArtistExists_ReturnsArtist()
    {
        var artist = new Artist("Magdalena Bay", "US");
        var request = new GetArtistByIdQuery(artist.Id);

        _repository.GetByIdAsync(artist.Id, Arg.Any<CancellationToken>())
            .Returns(artist);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().NotBeNull();
        result!.Id.Should().Be(artist.Id);
        result.Name.Should().Be("Magdalena Bay");
        result.Country.Should().Be("US");
    }

    [Test]
    public async Task Handle_WhenArtistDoesNotExist_ReturnsNull()
    {
        var artistId = Guid.NewGuid();
        var request = new GetArtistByIdQuery(artistId);

        _repository.GetByIdAsync(artistId, Arg.Any<CancellationToken>())
            .Returns((Artist?)null);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().BeNull();
    }
}
