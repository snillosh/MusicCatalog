using FluentAssertions;
using MusicCatalog.Application.Artists;
using MusicCatalog.Application.Artists.DeleteArtist;
using MusicCatalog.Domain.Artists;
using NSubstitute;
using NUnit.Framework;

namespace MusicCatalog.Application.Tests.Artists;

[TestFixture]
public class DeleteArtistHandlerTests
{
    private readonly IArtistRepository _repository = Substitute.For<IArtistRepository>();
    private readonly DeleteArtistHandler _handler;

    public DeleteArtistHandlerTests()
    {
        _handler = new DeleteArtistHandler(_repository);
    }

    [Test]
    public async Task Handle_WhenArtistExists_ReturnsTrue()
    {
        var artist = new Artist("Magdalena Bay", "US");
        var request = new DeleteArtistCommand(artist.Id);

        _repository.GetByIdTrackedAsync(artist.Id, Arg.Any<CancellationToken>())
            .Returns(artist);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().BeTrue();

        await _repository.Received(1)
            .DeleteAsync(artist, Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Handle_WhenArtistDoesNotExist_ReturnsFalse()
    {
        var artistId = Guid.NewGuid();
        var request = new DeleteArtistCommand(artistId);

        _repository.GetByIdTrackedAsync(artistId, Arg.Any<CancellationToken>())
            .Returns((Artist?)null);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().BeFalse();

        await _repository.DidNotReceive()
            .DeleteAsync(Arg.Any<Artist>(), Arg.Any<CancellationToken>());
    }
}
