using MediatR;
using MusicCatalog.Application.Common.Results;
using MusicCatalog.Contracts.Albums;

namespace MusicCatalog.Application.Albums.CreateAlbum;

public sealed record CreateAlbumCommand(Guid ArtistId, string Title, int? ReleaseYear) : IRequest<Result<AlbumDto>>;
