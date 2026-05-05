using MediatR;
using MusicCatalog.Contracts.Albums;
using MusicCatalog.Contracts.Common.Paging;

namespace MusicCatalog.Application.Albums.ListAlbumsByArtist;

public sealed record ListAlbumsByArtistQuery(Guid ArtistId, int Page = 1, int PageSize = 50) : IRequest<PagedResult<AlbumListItemDto>>;
