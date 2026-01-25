using MediatR;
using MusicCatalog.Application.Albums.Dto;

namespace MusicCatalog.Application.Albums.ListAlbums;

public sealed record ListAlbumsQuery : IRequest<IReadOnlyList<AlbumListItemDto>>;
