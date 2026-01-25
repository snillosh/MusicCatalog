using MediatR;
using MusicCatalog.Application.Albums.Dto;

namespace MusicCatalog.Application.Albums.ListAlbums;

public sealed class ListAlbumsHandler(IAlbumRepository albumRepository)
    : IRequestHandler<ListAlbumsQuery, IReadOnlyList<AlbumListItemDto>>
{
    public async Task<IReadOnlyList<AlbumListItemDto>> Handle(ListAlbumsQuery request,
        CancellationToken cancellationToken) =>
        (await albumRepository.GetAllWithArtistNameAsync(cancellationToken)).ToList();
}
