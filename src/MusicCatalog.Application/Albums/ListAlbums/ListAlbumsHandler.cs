using MediatR;
using MusicCatalog.Contracts.Albums;
using MusicCatalog.Contracts.Common.Paging;

namespace MusicCatalog.Application.Albums.ListAlbums;

public sealed class ListAlbumsHandler(IAlbumRepository albumRepository)
    : IRequestHandler<ListAlbumsQuery, PagedResult<AlbumListItemDto>>
{
    public async Task<PagedResult<AlbumListItemDto>> Handle(ListAlbumsQuery request,
        CancellationToken cancellationToken) =>
        await albumRepository.GetAllWithArtistNameAsync(request.Page, request.PageSize, request.ReleasedAfter,
        cancellationToken);
}
