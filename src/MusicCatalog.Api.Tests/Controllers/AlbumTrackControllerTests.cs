using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicCatalog.Api.Controllers;
using MusicCatalog.Application.Common.Results;
using MusicCatalog.Application.Tracks.CreateTrack;
using MusicCatalog.Application.Tracks.ListTracksByAlbum;
using MusicCatalog.Contracts.Tracks;
using NSubstitute;
using NUnit.Framework;

namespace MusicCatalog.Api.Tests.Controllers;

[TestFixture]
public class AlbumTrackControllerTests
{
    private readonly ISender _sender = Substitute.For<ISender>();
    private readonly AlbumTracksController _controller;

    public AlbumTrackControllerTests()
    {
        _controller = new AlbumTracksController(_sender);
    }

    [Test]
    public async Task GetAllTracksByAlbumId_WhenAlbumExists_ReturnsOk()
    {
        var albumId = Guid.NewGuid();

        var tracks = new List<TrackDto>
        {
            new(Guid.NewGuid(), albumId, 3, "California", 60), new(Guid.NewGuid(), albumId, 4, "Flesh WithoutBlood", 60)
        };

        _sender.Send(
            Arg.Is<ListTracksByAlbumQuery>(q => q.AlbumId == albumId),
            Arg.Any<CancellationToken>())
            .Returns(tracks);

        var result = await _controller.GetAllTracksByAlbumId(albumId, CancellationToken.None);

        result.Result.Should()
            .BeOfType<OkObjectResult>()
            .Which.Value.Should()
            .BeEquivalentTo(tracks);
    }

    [Test]
    public async Task GetAllTracksByAlbumId_WhenNotFound_ReturnsOkWithEmptyCollection()
    {
        var albumId = Guid.NewGuid();

        _sender.Send(
            Arg.Is<ListTracksByAlbumQuery>(q => q.AlbumId == albumId),
            Arg.Any<CancellationToken>())
            .Returns([]);

        var result = await _controller.GetAllTracksByAlbumId(albumId, CancellationToken.None);

        result.Result.Should().BeOfType<OkObjectResult>().Which.Value.Should().BeEquivalentTo(Array.Empty<TrackDto>());
    }

    [Test]
    public async Task Create_WithValidRequest_ReturnsOk()
    {
        var albumId = Guid.NewGuid();
        var request = new CreateTrackRequest(1, "California", 60);
        var track = new TrackDto(Guid.NewGuid(), albumId, 1, "California", 60);

        _sender.Send(Arg.Any<CreateTrackCommand>(), Arg.Any<CancellationToken>())
            .Returns(Result<TrackDto>.Success(track));

        var result = await _controller.Create(albumId, request, CancellationToken.None);

        var objectResult = result.Result.Should()
            .BeOfType<ObjectResult>()
            .Subject;

        objectResult.StatusCode.Should().Be(StatusCodes.Status201Created);

        objectResult.Value.Should().BeEquivalentTo(track);
    }

    [Test]
    public async Task Create_WhenAlbumDoesNotExist_ReturnsNotFound()
    {
        var albumId = Guid.NewGuid();

        var request = new CreateTrackRequest(1, "California", 60);

        _sender
            .Send(
            Arg.Any<CreateTrackCommand>(),
            Arg.Any<CancellationToken>())
            .Returns(
            Result<TrackDto>.Fail(
            "albums.notFound",
            "Album not found."));

        var result = await _controller.Create(albumId, request, CancellationToken.None);

        var objectResult = result.Result.Should()
            .BeOfType<ObjectResult>()
            .Subject;

        objectResult.StatusCode.Should().Be(StatusCodes.Status404NotFound);

        var problem = objectResult.Value.Should()
            .BeOfType<ProblemDetails>()
            .Subject;

        problem.Title.Should().Be("albums.notFound");
        problem.Detail.Should().Be("Album not found.");
    }

    [Test]
    public async Task Create_WhenTrackNumberAlreadyExists_ReturnsConflict()
    {
        var albumId = Guid.NewGuid();

        var request = new CreateTrackRequest(1, "California", 60);

        _sender
            .Send(
            Arg.Any<CreateTrackCommand>(),
            Arg.Any<CancellationToken>())
            .Returns(
            Result<TrackDto>.Fail(
            "tracks.duplicateTrackNumber",
            "Duplicate track number."));

        var result = await _controller.Create(
        albumId,
        request,
        CancellationToken.None);

        var objectResult = result.Result.Should()
            .BeOfType<ObjectResult>()
            .Subject;

        objectResult.StatusCode.Should().Be(StatusCodes.Status409Conflict);
    }
}
