using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicCatalog.Application.Metadata.GetAlbumById;
using MusicCatalog.Application.Metadata.GetByQuery;
using MusicCatalog.Contracts.Albums;

namespace MusicCatalog.Api.Controllers;

[ApiController]
[Route("api/metadata")]
public class MetadataController(ISender sender) : ControllerBase
{
    [AllowAnonymous]
    [HttpGet("{query}")]
    public async Task<ActionResult<IReadOnlyList<AlbumGroupSearchResult>>> GetByQuery(
        string query,
        CancellationToken ct)
    {
        var queryResults = await sender.Send(new GetByQueryQuery(query), ct);
        return Ok(queryResults);
    }

    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<AlbumImportPreview?>> GetAlbumById(Guid id, CancellationToken ct)
    {
        var result = await sender.Send(new GetAlbumMetadataByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }
}
