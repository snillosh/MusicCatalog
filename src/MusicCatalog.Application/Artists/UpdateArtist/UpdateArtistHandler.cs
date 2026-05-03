using MediatR;
using MusicCatalog.Contracts.Artists;

namespace MusicCatalog.Application.Artists.UpdateArtist;

public sealed class UpdateArtistHandler(IArtistRepository repo) : IRequestHandler<UpdateArtistCommand, ArtistDto?>
{
    public async Task<ArtistDto?> Handle(UpdateArtistCommand request, CancellationToken ct)
    {
        var artist = await repo.GetByIdTrackedAsync(request.Id, ct);
        if (artist is null) return null;

        var newName = request.Name.Trim();
        var newCountry = request.Country;

        if (await repo.ExistsByNameAsync(newName, request.Id, ct))
            throw new InvalidOperationException("artists.duplicate");

        artist.Rename(newName);

        artist.SetCountry(newCountry);

        await repo.UpdateAsync(artist, ct);

        return new ArtistDto(artist.Id, artist.Name, artist.Country);
    }
}
