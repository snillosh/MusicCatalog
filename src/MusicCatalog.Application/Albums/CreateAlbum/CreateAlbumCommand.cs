using MediatR;
using MusicCatalog.Application.Albums.Dto;
using MusicCatalog.Application.Common.Results;

namespace MusicCatalog.Application.Albums.CreateAlbum;

public sealed record CreateAlbumCommand(Guid ArtistId, string Title, int? ReleaseYear) : IRequest<Result<AlbumDto>>;
