using MediatR;
using MusicCatalog.Application.Artists.Dto;

namespace MusicCatalog.Application.Artists.GetArtistById;

public sealed record GetArtistByIdQuery(Guid Id) : IRequest<ArtistDto?>;
