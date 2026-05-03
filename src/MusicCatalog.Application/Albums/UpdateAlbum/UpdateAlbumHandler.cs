using MediatR;
using MusicCatalog.Contracts.Albums;

namespace MusicCatalog.Application.Albums.UpdateAlbum;

public class UpdateAlbumHandler(IAlbumRepository repository) : IRequestHandler<UpdateAlbumCommand, AlbumDto?>
{
    public async Task<AlbumDto?> Handle(UpdateAlbumCommand request, CancellationToken cancellationToken)
    {
        var album = await repository.GetByIdTrackedAsync(request.Id, cancellationToken);
        if (album is null) return null;

        var newTitle = request.Title.Trim();

        if (await repository.ExistsByNameAsync(newTitle, request.Id, cancellationToken))
            throw new InvalidOperationException("albums.duplicate");

        album.Rename(newTitle);
        album.SetReleaseYear(request.ReleaseYear);

        await repository.UpdateAsync(album, cancellationToken);

        return new AlbumDto(album.Id, album.ArtistId, album.Title, album.ReleaseYear);
    }
}
