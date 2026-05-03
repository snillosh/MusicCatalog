using MediatR;
using MusicCatalog.Application.Common.Results;
using MusicCatalog.Contracts.Artists;
using MusicCatalog.Domain.Artists;

namespace MusicCatalog.Application.Artists.CreateArtist;

public class CreateArtistHandler(IArtistRepository repository) : IRequestHandler<CreateArtistCommand, Result<ArtistDto>>
{
    public async Task<Result<ArtistDto>> Handle(CreateArtistCommand request, CancellationToken cancellationToken)
    {
        var name = request.Name.Trim();

        if (await repository.ExistsByNameAsync(name, cancellationToken))
            return Result<ArtistDto>.Fail("artists.duplicate", "An artist with the same name already exists.");

        var artist = new Artist(name, request.Country);

        await repository.AddAsync(artist, cancellationToken);

        return Result<ArtistDto>.Success(new ArtistDto(artist.Id, artist.Name, artist.Country));
    }
}
