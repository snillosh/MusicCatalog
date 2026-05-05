using MediatR;
using MusicCatalog.Contracts.Albums;
using MusicCatalog.Contracts.Common.Paging;

namespace MusicCatalog.Application.Albums.ListAlbumsByArtist;

public sealed class ListAlbumsByArtistHandler(IAlbumRepository albums)
    : IRequestHandler<ListAlbumsByArtistQuery, PagedResult<AlbumListItemDto>>
{
    public async Task<PagedResult<AlbumListItemDto>> Handle(ListAlbumsByArtistQuery request, CancellationToken ct) => await albums.GetByArtistIdAsync(request.ArtistId, request.Page, request.PageSize, ct);
}
