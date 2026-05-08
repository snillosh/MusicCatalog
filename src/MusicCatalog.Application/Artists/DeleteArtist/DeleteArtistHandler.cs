using MediatR;

namespace MusicCatalog.Application.Artists.DeleteArtist;

public sealed class DeleteArtistHandler(IArtistRepository repo) : IRequestHandler<DeleteArtistCommand, bool>
{
    public async Task<bool> Handle(DeleteArtistCommand request, CancellationToken ct)
    {
        var artist = await repo.GetByIdTrackedAsync(request.Id, ct);
        if (artist is null)
        {
            return false;
        }

        await repo.DeleteAsync(artist, ct);
        return true;
    }
}
