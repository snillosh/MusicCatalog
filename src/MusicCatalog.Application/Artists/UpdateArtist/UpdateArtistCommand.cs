using MediatR;
using MusicCatalog.Application.Artists.Dto;

namespace MusicCatalog.Application.Artists.UpdateArtist;

public sealed record UpdateArtistCommand(Guid Id, string Name, string? Country) : IRequest<ArtistDto>;
