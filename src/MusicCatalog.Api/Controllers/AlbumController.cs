using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicCatalog.Application.Albums.ListAlbums;

namespace MusicCatalog.Api.Controllers;

[ApiController]
[Route("api/albums")]
public class AlbumController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50,
        [FromQuery] int? releasedAfter = null,
        CancellationToken cancellationToken = default) =>
        Ok(await sender.Send(new ListAlbumsQuery(page, pageSize, releasedAfter), cancellationToken));
}
