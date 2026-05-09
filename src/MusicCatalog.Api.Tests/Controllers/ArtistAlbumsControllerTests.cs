using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicCatalog.Api.Controllers;
using MusicCatalog.Application.Albums.CreateAlbum;
using MusicCatalog.Application.Albums.ListAlbumsByArtist;
using MusicCatalog.Application.Common.Results;
using MusicCatalog.Contracts.Albums;
using MusicCatalog.Contracts.Common.Paging;
using NSubstitute;
using NUnit.Framework;

namespace MusicCatalog.Api.Tests.Controllers;

[TestFixture]
public class ArtistAlbumsControllerTests
{
    private readonly ISender _sender = Substitute.For<ISender>();
    private readonly ArtistAlbumsController _controller;

    public ArtistAlbumsControllerTests()
    {
        _controller = new ArtistAlbumsController(_sender);
    }

    [Test]
    public async Task GetAllAlbumsByArtistId_WhenArtistExists_ReturnsOk()
    {
        var artistId = Guid.NewGuid();
        var artistName = "Grimes";

        var albums = new List<AlbumListItemDto>
        {
            new(Guid.NewGuid(), artistId, artistName, "Visions", 2012),
            new(Guid.NewGuid(), artistId, artistName, "Art Angels", 2015)

        };

        var pagedResult = new PagedResult<AlbumListItemDto>(albums, 1, 50, 1);

        _sender.Send(Arg.Is<ListAlbumsByArtistQuery>(q => q.ArtistId == artistId), Arg.Any<CancellationToken>())
            .Returns(pagedResult);

        var result = await _controller.GetAllAlbumsByArtistId(artistId);

        result.Result.Should()
            .BeOfType<OkObjectResult>()
            .Which.Value.Should()
            .BeEquivalentTo(pagedResult);
    }

    [Test]
    public async Task GetAllAlbumsByArtistId_WhenNotFound_ReturnsOkWithEmptyPagedResults()
    {
        var artistId = Guid.NewGuid();

        var pagedResult = new PagedResult<AlbumListItemDto>([], 1, 50, 1);

        _sender.Send(Arg.Is<ListAlbumsByArtistQuery>(q => q.ArtistId == artistId), Arg.Any<CancellationToken>())
            .Returns(pagedResult);

        var result = await _controller.GetAllAlbumsByArtistId(artistId);

        result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(pagedResult);
    }

    [Test]
    public async Task Create_WithValidRequest_ReturnsOk()
    {
        var artistId = Guid.NewGuid();

        var request = new CreateAlbumRequest("Art Angels", 2015);
        var album = new AlbumDto(Guid.NewGuid(), artistId, "Art Angels", 2015);

        _sender.Send(Arg.Any<CreateAlbumCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result<AlbumDto>.Success(album));

        var result = await _controller.Create(artistId, request, CancellationToken.None);

        var objectResult = result.Result.Should()
            .BeOfType<ObjectResult>()
            .Subject;

        objectResult.StatusCode.Should().Be(StatusCodes.Status201Created);

        objectResult.Value.Should().BeEquivalentTo(album);
    }

    [Test]
    public async Task Create_WhenArtistDoesNotExist_ReturnsNotFound()
    {
        var artistId = Guid.NewGuid();

        var request = new CreateAlbumRequest("Art Angels", 2015);

        _sender.Send(Arg.Any<CreateAlbumCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result<AlbumDto>.Fail("artists.notFound", "Artist not found."));

        var result = await _controller.Create(artistId, request, CancellationToken.None);

        var objectResult = result.Result.Should()
            .BeOfType<ObjectResult>()
            .Subject;

        objectResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);

        var problem = objectResult.Value.Should()
            .BeOfType<ProblemDetails>()
            .Subject;

        problem.Title.Should().Be("artists.notFound");
        problem.Detail.Should().Be("Artist not found.");
    }

    [Test]
    public async Task Create_WhenAlbumTitleAlreadyExists_ReturnsConflict()
    {
        var artistId = Guid.NewGuid();

        var request = new CreateAlbumRequest("Art Angels", 2015);

        _sender.Send(Arg.Any<CreateAlbumCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result<AlbumDto>.Fail("albums.duplicate", "That artist already has an album with that title."));

        var result = await _controller.Create(artistId, request, CancellationToken.None);

        var objectResult = result.Result.Should()
            .BeOfType<ObjectResult>()
            .Subject;

        objectResult.StatusCode.Should().Be(StatusCodes.Status409Conflict);
    }
}
