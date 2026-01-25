using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicCatalog.Application.Albums.DeleteAlbum;
using MusicCatalog.Application.Albums.GetAlbumById;
using MusicCatalog.Application.Albums.ListAlbums;
using MusicCatalog.Application.Albums.UpdateAlbum;

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

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var dto = await sender.Send(new GetAlbumByIdQuery(id), ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var deleted = await sender.Send(new DeleteAlbumCommand(id), ct);
        return deleted ? NoContent() : NotFound();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateAlbumRequest request, CancellationToken ct)
    {
        var dto = await sender.Send(new UpdateAlbumCommand(id, request.Title, request.ReleaseYear), ct);
        return dto is null ? NotFound() : Ok(dto);
    }

    public sealed record UpdateAlbumRequest(string Title, int? ReleaseYear);
}
