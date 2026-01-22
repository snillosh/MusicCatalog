using MediatR;
using MusicCatalog.Application.Artists.Dto;

namespace MusicCatalog.Application.Artists.ListArtists;

public sealed record ListArtistsQuery : IRequest<IReadOnlyList<ArtistDto>>;
