using MediatR;
using MusicCatalog.Application.Albums.Dto;

namespace MusicCatalog.Application.Albums.GetAlbumById;

public class GetAlbumByIdHandler(IAlbumRepository repository) : IRequestHandler<GetAlbumByIdQuery, AlbumDto?>
{
    public async Task<AlbumDto?> Handle(GetAlbumByIdQuery request, CancellationToken cancellationToken)
    {
        var album = await repository.GetByIdAsync(request.Id, cancellationToken);
        return album is null ? null : new AlbumDto(album.Id, album.ArtistId, album.Title, album.ReleaseYear);
    }
}
