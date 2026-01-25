using MediatR;

namespace MusicCatalog.Application.Albums.DeleteAlbum;

public sealed record DeleteAlbumCommand(Guid Id) : IRequest<bool>;
