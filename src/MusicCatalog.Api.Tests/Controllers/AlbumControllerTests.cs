using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicCatalog.Api.Controllers;
using MusicCatalog.Application.Albums.DeleteAlbum;
using MusicCatalog.Application.Albums.GetAlbumById;
using MusicCatalog.Application.Albums.ListAlbums;
using MusicCatalog.Application.Albums.UpdateAlbum;
using MusicCatalog.Contracts.Albums;
using MusicCatalog.Contracts.Common.Paging;
using NSubstitute;
using NUnit.Framework;

namespace MusicCatalog.Api.Tests.Controllers;

[TestFixture]
public class AlbumControllerTests
{
    private readonly ISender _sender = Substitute.For<ISender>();
    private readonly AlbumController _controller;

    public AlbumControllerTests()
    {
        _controller = new AlbumController(_sender);
    }

    [Test]
    public async Task List_ReturnsOk_WithPagedAlbums()
    {
        var albums = new PagedResult<AlbumListItemDto>(
        [
            new AlbumListItemDto(
            Guid.NewGuid(),
            Guid.NewGuid(),
            "Grimes",
            "Art Angels",
            2015)
        ],
        1,
        50,
        1);

        _sender
            .Send(
            Arg.Is<ListAlbumsQuery>(q =>
                q.Page == 1 && q.PageSize == 50 && q.ReleasedAfter == 1990),
            Arg.Any<CancellationToken>())
            .Returns(albums);

        var result = await _controller.List(1, 50, 1990, CancellationToken.None);

        result.Result.Should()
            .BeOfType<OkObjectResult>()
            .Which.Value.Should()
            .BeEquivalentTo(albums);
    }

    [Test]
    public async Task GetById_WhenAlbumExists_ReturnsOk()
    {
        var albumId = Guid.NewGuid();

        var album = new AlbumDto(
        albumId,
        Guid.NewGuid(),
        "Art Angels",
        2015);

        _sender
            .Send(
            Arg.Is<GetAlbumByIdQuery>(q => q.Id == albumId),
            Arg.Any<CancellationToken>())
            .Returns(album);

        var result = await _controller.GetById(albumId, CancellationToken.None);

        result.Result.Should()
            .BeOfType<OkObjectResult>()
            .Which.Value.Should()
            .BeEquivalentTo(album);
    }

    [Test]
    public async Task GetById_WhenNotFound_ReturnsNotFound()
    {
        var albumId = Guid.NewGuid();

        _sender
            .Send(
            Arg.Is<GetAlbumByIdQuery>(q => q.Id == albumId),
            Arg.Any<CancellationToken>())
            .Returns((AlbumDto?)null);

        var result = await _controller.GetById(albumId, CancellationToken.None);

        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task Delete_WhenExists_ReturnsNoContent()
    {
        var albumId = Guid.NewGuid();

        _sender
            .Send(
            Arg.Is<DeleteAlbumCommand>(c => c.Id == albumId),
            Arg.Any<CancellationToken>())
            .Returns(true);

        var result = await _controller.Delete(albumId, CancellationToken.None);

        result.Result.Should().BeOfType<NoContentResult>();
    }

    [Test]
    public async Task Delete_WhenNotFound_ReturnsNotFound()
    {
        var albumId = Guid.NewGuid();

        _sender
            .Send(
            Arg.Is<DeleteAlbumCommand>(c => c.Id == albumId),
            Arg.Any<CancellationToken>())
            .Returns(false);

        var result = await _controller.Delete(albumId, CancellationToken.None);

        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task Update_WhenExists_ReturnsOk()
    {
        var albumId = Guid.NewGuid();

        var request = new UpdateAlbumRequest("Kid A", 2000);

        var album = new AlbumDto(
        albumId,
        Guid.NewGuid(),
        "Visions",
        2012);

        _sender
            .Send(
            Arg.Is<UpdateAlbumCommand>(c =>
                c.Id == albumId && c.Title == request.Title && c.ReleaseYear == request.ReleaseYear),
            Arg.Any<CancellationToken>())
            .Returns(album);

        var result = await _controller.Update(albumId, request, CancellationToken.None);

        result.Result.Should()
            .BeOfType<OkObjectResult>()
            .Which.Value.Should()
            .BeEquivalentTo(album);
    }

    [Test]
    public async Task Update_WhenNotFound_ReturnsNotFound()
    {
        var albumId = Guid.NewGuid();

        var request = new UpdateAlbumRequest("Visions", 2012);

        _sender
            .Send(
            Arg.Is<UpdateAlbumCommand>(c =>
                c.Id == albumId && c.Title == request.Title && c.ReleaseYear == request.ReleaseYear),
            Arg.Any<CancellationToken>())
            .Returns((AlbumDto?)null);

        var result = await _controller.Update(albumId, request, CancellationToken.None);

        result.Result.Should().BeOfType<NotFoundResult>();
    }
}
