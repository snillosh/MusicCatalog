using MediatR;
using MusicCatalog.Contracts.Artists;

namespace MusicCatalog.Application.Artists.UpdateArtist;

public sealed record UpdateArtistCommand(Guid Id, string Name, string? Country) : IRequest<ArtistDto>;
