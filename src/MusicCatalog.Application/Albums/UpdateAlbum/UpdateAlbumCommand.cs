using MediatR;
using MusicCatalog.Application.Albums.Dto;

namespace MusicCatalog.Application.Albums.UpdateAlbum;

public sealed record UpdateAlbumCommand(Guid Id, string Title, int? ReleaseYear) : IRequest<AlbumDto>;
