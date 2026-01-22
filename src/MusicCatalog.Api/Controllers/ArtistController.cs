using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicCatalog.Application.Artists;
using MusicCatalog.Application.Artists.CreateArtist;
using MusicCatalog.Application.Artists.Dto;
using MusicCatalog.Application.Artists.ListArtists;

namespace MusicCatalog.Api.Controllers;

[ApiController]
[Route("api/artists")]
public sealed class ArtistController(ISender sender, IArtistRepository repo) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(CancellationToken cancellationToken) =>
        Ok(await sender.Send(new ListArtistsQuery(), cancellationToken));

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ArtistDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var artist = await repo.GetByIdAsync(id, cancellationToken);
        if (artist is null) return NotFound();
        return Ok(new ArtistDto(artist.Id, artist.Name));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateArtistRequest request, CancellationToken cancellationToken)
    {
        var result = await sender.Send(new CreateArtistCommand(request.Name), cancellationToken);

        if (!result.IsSuccess)
            return Conflict(new ProblemDetails
            {
                Title = result.Error!.Code,
                Detail = result.Error.Message,
                Status = StatusCodes.Status409Conflict
            });

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value);
    }

    public sealed record CreateArtistRequest(string Name);
}
