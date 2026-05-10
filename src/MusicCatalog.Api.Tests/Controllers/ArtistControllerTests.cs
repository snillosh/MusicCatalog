using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicCatalog.Api.Controllers;
using MusicCatalog.Application.Artists.DeleteArtist;
using MusicCatalog.Application.Artists.GetArtistById;
using MusicCatalog.Application.Artists.UpdateArtist;
using MusicCatalog.Contracts.Artists;
using NSubstitute;
using NUnit.Framework;

namespace MusicCatalog.Api.Tests.Controllers;

[TestFixture]
public class ArtistControllerTests
{
    private readonly ISender _sender = Substitute.For<ISender>();
    private readonly ArtistController _controller;

    public ArtistControllerTests()
    {
        _controller = new ArtistController(_sender);
    }

    [Test]
    public async Task GetById_WhenArtistExists_ReturnsOk()
    {
        var artistId = Guid.NewGuid();

        var artist = new ArtistDto(
        artistId,
        "Magdalena Bay",
        "US");

        _sender
            .Send(
            Arg.Is<GetArtistByIdQuery>(q => q.Id == artistId),
            Arg.Any<CancellationToken>())
            .Returns(artist);

        var result = await _controller.GetById(artistId, CancellationToken.None);

        result.Result.Should()
            .BeOfType<OkObjectResult>()
            .Which.Value.Should()
            .BeEquivalentTo(artist);
    }

    [Test]
    public async Task GetById_WhenNotFound_ReturnsNotFound()
    {
        var artistId = Guid.NewGuid();

        _sender
            .Send(
            Arg.Is<GetArtistByIdQuery>(q => q.Id == artistId),
            Arg.Any<CancellationToken>())
            .Returns((ArtistDto?)null);

        var result = await _controller.GetById(artistId, CancellationToken.None);

        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task Delete_WhenExists_ReturnsNoContent()
    {
        var artistId = Guid.NewGuid();

        _sender
            .Send(
            Arg.Is<DeleteArtistCommand>(c => c.Id == artistId),
            Arg.Any<CancellationToken>())
            .Returns(true);

        var result = await _controller.Delete(artistId, CancellationToken.None);

        result.Result.Should().BeOfType<NoContentResult>();
    }

    [Test]
    public async Task Delete_WhenNotFound_ReturnsNotFound()
    {
        var artistId = Guid.NewGuid();

        _sender
            .Send(
            Arg.Is<DeleteArtistCommand>(c => c.Id == artistId),
            Arg.Any<CancellationToken>())
            .Returns(false);

        var result = await _controller.Delete(artistId, CancellationToken.None);

        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task Update_WhenExists_ReturnsOk()
    {
        var artistId = Guid.NewGuid();

        var request = new UpdateArtistRequest("Magdalena Bay", "US");

        var artist = new ArtistDto(
        artistId,
        "Magdalena Bay",
        "US");

        _sender
            .Send(
            Arg.Is<UpdateArtistCommand>(c =>
                c.Id == artistId && c.Name == request.Name && c.Country == request.Country),
            Arg.Any<CancellationToken>())
            .Returns(artist);

        var result = await _controller.Update(artistId, request, CancellationToken.None);

        result.Result.Should()
            .BeOfType<OkObjectResult>()
            .Which.Value.Should()
            .BeEquivalentTo(artist);
    }

    [Test]
    public async Task Update_WhenNotFound_ReturnsNotFound()
    {
        var artistId = Guid.NewGuid();

        var request = new UpdateArtistRequest("Magdalena Bay", "US");

        _sender
            .Send(
            Arg.Is<UpdateArtistCommand>(c =>
                c.Id == artistId && c.Name == request.Name && c.Country == request.Country),
            Arg.Any<CancellationToken>())
            .Returns((ArtistDto?)null);

        var result = await _controller.Update(artistId, request, CancellationToken.None);

        result.Result.Should().BeOfType<NotFoundResult>();
    }
}
