using MediatR;

namespace MusicCatalog.Application.Artists.DeleteArtist;

public sealed record DeleteArtistCommand(Guid Id) : IRequest<bool>;
