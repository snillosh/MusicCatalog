using FluentAssertions;
using MusicCatalog.Application.Artists;
using MusicCatalog.Application.Artists.UpdateArtist;
using MusicCatalog.Domain.Artists;
using NSubstitute;
using NUnit.Framework;

namespace MusicCatalog.Application.Tests.Artists;

[TestFixture]
public class UpdateArtistHandlerTests
{
    private readonly IArtistRepository _repository = Substitute.For<IArtistRepository>();
    private readonly UpdateArtistHandler _handler;

    public UpdateArtistHandlerTests()
    {
        _handler = new UpdateArtistHandler(_repository);
    }

    [Test]
    public async Task Handle_WithValidArtist_ReturnsUpdatedArtist()
    {
        var artist = new Artist("Magdalena Bay", "US");
        var request = new UpdateArtistCommand(artist.Id, "  Mag Bay  ", "US");

        _repository.GetByIdTrackedAsync(artist.Id, Arg.Any<CancellationToken>())
            .Returns(artist);

        _repository.ExistsByNameAsync("Mag Bay", artist.Id, Arg.Any<CancellationToken>())
            .Returns(false);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().NotBeNull();
        result!.Id.Should().Be(artist.Id);
        result.Name.Should().Be("Mag Bay");
        result.Country.Should().Be("US");

        await _repository.Received(1)
            .UpdateAsync(
            Arg.Is<Artist>(a =>
                a.Id == artist.Id && a.Name == "Mag Bay" && a.Country == "US"),
            Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Handle_WhenArtistDoesNotExist_ReturnsNull()
    {
        var artistId = Guid.NewGuid();
        var request = new UpdateArtistCommand(artistId, "Mag Bay", "US");

        _repository.GetByIdTrackedAsync(artistId, Arg.Any<CancellationToken>())
            .Returns((Artist?)null);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().BeNull();

        await _repository.DidNotReceive()
            .ExistsByNameAsync(Arg.Any<string>(), Arg.Any<Guid>(), Arg.Any<CancellationToken>());

        await _repository.DidNotReceive()
            .UpdateAsync(Arg.Any<Artist>(), Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Handle_WithDuplicateName_ThrowsInvalidOperationException()
    {
        var artist = new Artist("Magdalena Bay", "US");
        var request = new UpdateArtistCommand(artist.Id, "  Cocteau Twins  ", "UK");

        _repository.GetByIdTrackedAsync(artist.Id, Arg.Any<CancellationToken>())
            .Returns(artist);

        _repository.ExistsByNameAsync("Cocteau Twins", artist.Id, Arg.Any<CancellationToken>())
            .Returns(true);

        await FluentActions
            .Invoking(() => _handler.Handle(request, CancellationToken.None))
            .Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("artists.duplicate");

        await _repository.DidNotReceive()
            .UpdateAsync(Arg.Any<Artist>(), Arg.Any<CancellationToken>());
    }
}
