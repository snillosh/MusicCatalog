using MediatR;

namespace MusicCatalog.Application.Albums.DeleteAlbum;

public class DeleteAlbumHandler(IAlbumRepository repository) : IRequestHandler<DeleteAlbumCommand, bool>
{
    public async Task<bool> Handle(DeleteAlbumCommand request, CancellationToken cancellationToken)
    {
        var album = await repository.GetByIdTrackedAsync(request.Id, cancellationToken);

        if (album is null) return false;

        await repository.DeleteAsync(album, cancellationToken);
        return true;
    }
}
