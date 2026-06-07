using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicCatalog.Application.Importing;
using MusicCatalog.Contracts.Imports;

namespace MusicCatalog.Api.Controllers;

[ApiController]
[Route("api/albums/import")]
public class AlbumImportController(ISender sender) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult> Import([FromBody] ImportAlbumRequest request, CancellationToken ct)
    {
        var result = await sender.Send(new ImportAlbumCommand(request.ReleaseGroupId), ct);

        if (!result.IsSuccess)
        {
            var status = result.Error!.Code switch
            {
                "album.notFound" => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status400BadRequest
            };

            return StatusCode(
            status,
            new ProblemDetails
            {
                Title = result.Error.Code, Detail = result.Error.Message
            });
        }

        return StatusCode(StatusCodes.Status201Created, result.Value);
    }
}
