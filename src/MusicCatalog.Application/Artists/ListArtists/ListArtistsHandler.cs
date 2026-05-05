using MediatR;
using MusicCatalog.Contracts.Artists;
using MusicCatalog.Contracts.Common.Paging;

namespace MusicCatalog.Application.Artists.ListArtists;

public sealed class ListArtistsHandler(IArtistRepository repository)
    : IRequestHandler<ListArtistsQuery, PagedResult<ArtistDto>>
{
    public async Task<PagedResult<ArtistDto>> Handle(ListArtistsQuery request, CancellationToken cancellationToken) =>
        await repository.GetAllAsync(request.Page, request.PageSize, cancellationToken);
}
