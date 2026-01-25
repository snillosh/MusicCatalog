using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicCatalog.Application.Tracks.CreateTrack;
using MusicCatalog.Application.Tracks.ListTracksByAlbum;

namespace MusicCatalog.Api.Controllers;

[ApiController]
[Route("api/albums/{albumId:guid}/tracks")]
public sealed class AlbumTracksController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(Guid albumId, CancellationToken ct)
        => Ok(await sender.Send(new ListTracksByAlbumQuery(albumId), ct));

    [HttpPost]
    public async Task<IActionResult> Create(Guid albumId, [FromBody] CreateTrackRequest request, CancellationToken ct)
    {
        var result = await sender.Send(
            new CreateTrackCommand(albumId, request.TrackNumber, request.Title, request.DurationSeconds),
            ct);

        if (!result.IsSuccess)
        {
            var status = result.Error!.Code switch
            {
                "albums.notFound" => StatusCodes.Status404NotFound,
                "tracks.duplicateTrackNumber" => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status400BadRequest
            };

            return StatusCode(status, new ProblemDetails { Title = result.Error.Code, Detail = result.Error.Message });
        }

        return StatusCode(StatusCodes.Status201Created, result.Value);
    }

    public sealed record CreateTrackRequest(int TrackNumber, string Title, int? DurationSeconds);
}
