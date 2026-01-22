using MediatR;
using MusicCatalog.Application.Artists.Dto;

namespace MusicCatalog.Application.Artists.ListArtists;

public sealed class ListArtistsHandler(IArtistRepository repository)
    : IRequestHandler<ListArtistsQuery, IReadOnlyList<ArtistDto>>
{
    public async Task<IReadOnlyList<ArtistDto>> Handle(ListArtistsQuery request, CancellationToken cancellationToken) =>
        (await repository.GetAllAsync(cancellationToken)).Select(a => new ArtistDto(a.Id, a.Name)).ToList();
}
