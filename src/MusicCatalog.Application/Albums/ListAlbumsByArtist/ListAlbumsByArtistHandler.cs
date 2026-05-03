using MediatR;
using MusicCatalog.Contracts.Albums;

namespace MusicCatalog.Application.Albums.ListAlbumsByArtist;

public sealed class ListAlbumsByArtistHandler(IAlbumRepository albums)
    : IRequestHandler<ListAlbumsByArtistQuery, IReadOnlyList<AlbumDto>>
{
    public async Task<IReadOnlyList<AlbumDto>> Handle(ListAlbumsByArtistQuery request, CancellationToken ct)
    {
        var retrievedAlbums = await albums.GetByArtistIdAsync(request.ArtistId, ct);

        return retrievedAlbums.Select(a => new AlbumDto(a.Id, a.ArtistId, a.Title, a.ReleaseYear)).ToList();
    }
}
