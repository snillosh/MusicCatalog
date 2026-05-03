using MediatR;
using MusicCatalog.Contracts.Albums;

namespace MusicCatalog.Application.Albums.UpdateAlbum;

public sealed record UpdateAlbumCommand(Guid Id, string Title, int? ReleaseYear) : IRequest<AlbumDto>;
