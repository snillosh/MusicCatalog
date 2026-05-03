using MediatR;
using MusicCatalog.Contracts.Albums;
using MusicCatalog.Contracts.Common.Paging;

namespace MusicCatalog.Application.Albums.ListAlbums;

public sealed record ListAlbumsQuery(
    int Page = 1,
    int PageSize = 50,
    int? ReleasedAfter = null) : IRequest<PagedResult<AlbumListItemDto>>;
