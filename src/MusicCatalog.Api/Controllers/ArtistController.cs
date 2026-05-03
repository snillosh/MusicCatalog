using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicCatalog.Application.Artists.CreateArtist;
using MusicCatalog.Application.Artists.DeleteArtist;
using MusicCatalog.Application.Artists.GetArtistById;
using MusicCatalog.Application.Artists.ListArtists;
using MusicCatalog.Application.Artists.UpdateArtist;
using MusicCatalog.Contracts.Artists;

namespace MusicCatalog.Api.Controllers;

[ApiController]
[Route("api/artists")]
public sealed class ArtistController(ISender sender) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> List(CancellationToken cancellationToken) =>
        Ok(await sender.Send(new ListArtistsQuery(), cancellationToken));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var dto = await sender.Send(new GetArtistByIdQuery(id), ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateArtistRequest request, CancellationToken ct)
    {
        var result = await sender.Send(new CreateArtistCommand(request.Name, request.Country), ct);
        if (!result.IsSuccess)
            return Conflict(new ProblemDetails { Title = result.Error!.Code, Detail = result.Error.Message });

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateArtistRequest request, CancellationToken ct)
    {
        var dto = await sender.Send(new UpdateArtistCommand(id, request.Name, request.Country), ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var deleted = await sender.Send(new DeleteArtistCommand(id), ct);
        return deleted ? NoContent() : NotFound();
    }
}
