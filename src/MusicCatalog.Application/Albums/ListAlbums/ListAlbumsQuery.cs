using MediatR;
using MusicCatalog.Application.Common.Paging;
using MusicCatalog.Contracts.Albums;

namespace MusicCatalog.Application.Albums.ListAlbums;

public sealed record ListAlbumsQuery(
    int Page = 1,
    int PageSize = 50,
    int? ReleasedAfter = null) : IRequest<PagedResult<AlbumListItemDto>>;
