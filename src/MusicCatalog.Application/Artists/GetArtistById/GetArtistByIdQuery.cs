using MediatR;
using MusicCatalog.Contracts.Artists;

namespace MusicCatalog.Application.Artists.GetArtistById;

public sealed record GetArtistByIdQuery(Guid Id) : IRequest<ArtistDto?>;
