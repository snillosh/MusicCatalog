using MediatR;
using MusicCatalog.Application.Albums.Dto;
using MusicCatalog.Application.Common.Paging;

namespace MusicCatalog.Application.Albums.ListAlbums;

public sealed record ListAlbumsQuery(
    int Page = 1,
    int PageSize = 50,
    int? ReleasedAfter = null) : IRequest<PagedResult<AlbumListItemDto>>;
