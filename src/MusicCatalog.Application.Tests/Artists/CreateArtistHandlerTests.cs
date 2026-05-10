using FluentAssertions;
using MusicCatalog.Application.Artists;
using MusicCatalog.Application.Artists.CreateArtist;
using MusicCatalog.Domain.Artists;
using NSubstitute;
using NUnit.Framework;

namespace MusicCatalog.Application.Tests.Artists;

[TestFixture]
public class CreateArtistHandlerTests
{
    private readonly IArtistRepository _repository = Substitute.For<IArtistRepository>();
    private readonly CreateArtistHandler _handler;

    public CreateArtistHandlerTests()
    {
        _handler = new CreateArtistHandler(_repository);
    }

    [Test]
    public async Task Handle_WithValidArtist_ReturnsSuccess()
    {
        var request = new CreateArtistCommand("  Magdalena Bay  ", "US");

        _repository.ExistsByNameAsync("Magdalena Bay", Arg.Any<CancellationToken>())
            .Returns(false);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Name.Should().Be("Magdalena Bay");
        result.Value.Country.Should().Be("US");

        await _repository.Received(1)
            .AddAsync(
            Arg.Is<Artist>(a =>
                a.Name == "Magdalena Bay" && a.Country == "US"),
            Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Handle_WithDuplicateName_ReturnsFailure()
    {
        var request = new CreateArtistCommand("  Magdalena Bay  ", "US");

        _repository.ExistsByNameAsync("Magdalena Bay", Arg.Any<CancellationToken>())
            .Returns(true);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error!.Code.Should().Be("artists.duplicate");
        result.Error.Message.Should().Be("An artist with the same name already exists.");

        await _repository.DidNotReceive()
            .AddAsync(Arg.Any<Artist>(), Arg.Any<CancellationToken>());
    }
}
