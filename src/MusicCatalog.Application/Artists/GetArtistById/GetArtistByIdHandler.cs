using MediatR;
using MusicCatalog.Application.Artists.Dto;

namespace MusicCatalog.Application.Artists.GetArtistById;

public sealed class GetArtistByIdHandler(IArtistRepository artistRepository)
    : IRequestHandler<GetArtistByIdQuery, ArtistDto?>
{
    public async Task<ArtistDto?> Handle(GetArtistByIdQuery request, CancellationToken cancellationToken)
    {
        var artist = await artistRepository.GetByIdAsync(request.Id, cancellationToken);
        return artist is null ? null : new ArtistDto(artist.Id, artist.Name,  artist.Country);
    }
}
