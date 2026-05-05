using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicCatalog.Application.Albums.CreateAlbum;
using MusicCatalog.Application.Albums.ListAlbumsByArtist;
using MusicCatalog.Contracts.Albums;

namespace MusicCatalog.Api.Controllers;

[ApiController]
[Route("api/artists/{artistId:guid}/albums")]
public sealed class ArtistAlbumsController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<AlbumDto>>> List(Guid artistId, CancellationToken ct)
        => Ok(await sender.Send(new ListAlbumsByArtistQuery(artistId), ct));

    [HttpPost]
    public async Task<ActionResult<AlbumDto>> Create(Guid artistId, [FromBody] CreateAlbumRequest request, CancellationToken ct)
    {
        var result = await sender.Send(new CreateAlbumCommand(artistId, request.Title, request.ReleaseYear), ct);

        if (!result.IsSuccess)
        {
            var status = result.Error!.Code switch
            {
                "artists.notFound" => StatusCodes.Status404NotFound,
                "albums.duplicate" => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status400BadRequest
            };

            return StatusCode(status, new ProblemDetails { Title = result.Error.Code, Detail = result.Error.Message });
        }

        return StatusCode(StatusCodes.Status201Created, result.Value);
    }
}
