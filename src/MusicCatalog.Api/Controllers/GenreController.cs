using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicCatalog.Application.Genres.CreateGenre;
using MusicCatalog.Application.Genres.GetGenreById;

namespace MusicCatalog.Api.Controllers;

[ApiController]
[Route("api/genres")]
public class GenreController(ISender sender) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var dto = await sender.Send(new GetGenreByIdQuery(id), ct);
        return dto is null ? NotFound() : Ok(dto);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateGenreRequest request, CancellationToken ct)
    {
        var result = await sender.Send(new CreateGenreCommand(request.Title), ct);
        if (!result.IsSuccess)
            return Conflict(new ProblemDetails { Title = result.Error!.Code, Detail = result.Error.Message });

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value);
    }
}

public sealed record CreateGenreRequest(string Title);
