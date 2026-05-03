using MediatR;
using MusicCatalog.Contracts.Artists;

namespace MusicCatalog.Application.Artists.ListArtists;

public sealed record ListArtistsQuery : IRequest<IReadOnlyList<ArtistDto>>;
