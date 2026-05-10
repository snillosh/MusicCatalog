using FluentAssertions;
using MusicCatalog.Application.Genres;
using MusicCatalog.Application.Genres.CreateGenre;
using MusicCatalog.Domain.Genre;
using NSubstitute;
using NUnit.Framework;

namespace MusicCatalog.Application.Tests.Genres;

[TestFixture]
public class CreateGenreHandlerTests
{
    private readonly IGenreRepository _repository = Substitute.For<IGenreRepository>();
    private readonly CreateGenreHandler _handler;

    public CreateGenreHandlerTests()
    {
        _handler = new CreateGenreHandler(_repository);
    }

    [Test]
    public async Task Handle_WithValidGenre_ReturnsSuccess()
    {
        var request = new CreateGenreCommand("  Dream Pop  ");

        _repository.ExistsWithTitleAsync("Dream Pop", Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(false);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Title.Should().Be("Dream Pop");

        await _repository.Received(1)
            .AddAsync(
            Arg.Is<Genre>(g => g.Title == "Dream Pop"),
            Arg.Any<CancellationToken>());
    }

    [Test]
    public async Task Handle_WithDuplicateTitle_ReturnsFailure()
    {
        var request = new CreateGenreCommand("  Dream Pop  ");

        _repository.ExistsWithTitleAsync("Dream Pop", Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(true);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
        result.Error!.Code.Should().Be("genres.duplicate");
        result.Error.Message.Should().Be("There is already a genre with that title.");

        await _repository.DidNotReceive()
            .AddAsync(Arg.Any<Genre>(), Arg.Any<CancellationToken>());
    }
}
