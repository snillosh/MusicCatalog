using MediatR;
using MusicCatalog.Contracts.Albums;

namespace MusicCatalog.Application.Albums.GetAlbumById;

public sealed record GetAlbumByIdQuery(Guid Id) : IRequest<AlbumDto?>;
