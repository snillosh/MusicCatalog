using FluentAssertions;
using MusicCatalog.Application.Genres;
using MusicCatalog.Application.Genres.GetGenreById;
using MusicCatalog.Domain.Genre;
using NSubstitute;
using NUnit.Framework;

namespace MusicCatalog.Application.Tests.Genres;

[TestFixture]
public class GetGenreByIdHandlerTests
{
    private readonly IGenreRepository _repository = Substitute.For<IGenreRepository>();
    private readonly GetGenreByIdHandler _handler;

    public GetGenreByIdHandlerTests()
    {
        _handler = new GetGenreByIdHandler(_repository);
    }

    [Test]
    public async Task Handle_WhenGenreExists_ReturnsGenre()
    {
        var genre = new Genre("Dream Pop");
        var request = new GetGenreByIdQuery(genre.Id);

        _repository.GetByIdAsync(genre.Id, Arg.Any<CancellationToken>())
            .Returns(genre);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().NotBeNull();
        result!.Id.Should().Be(genre.Id);
        result.Title.Should().Be("Dream Pop");
    }

    [Test]
    public async Task Handle_WhenGenreDoesNotExist_ReturnsNull()
    {
        var genreId = Guid.NewGuid();
        var request = new GetGenreByIdQuery(genreId);

        _repository.GetByIdAsync(genreId, Arg.Any<CancellationToken>())
            .Returns((Genre?)null);

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().BeNull();
    }
}
