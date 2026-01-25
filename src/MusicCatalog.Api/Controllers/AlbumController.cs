using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicCatalog.Application.Albums.ListAlbums;

namespace MusicCatalog.Api.Controllers;

[ApiController]
[Route("api/albums")]
public class AlbumController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(CancellationToken cancellationToken) =>
        Ok(await sender.Send(new ListAlbumsQuery(), cancellationToken));
}
