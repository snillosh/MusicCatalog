using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicCatalog.Application.Artists.ListArtists;

namespace MusicCatalog.Api.Controllers;

[ApiController]
[Route("api/artists")]
public sealed class ArtistController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(CancellationToken cancellationToken) =>
        Ok(await sender.Send(new ListArtistsQuery(), cancellationToken));
}
