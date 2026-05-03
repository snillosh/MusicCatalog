using MediatR;
using MusicCatalog.Contracts.Albums;

namespace MusicCatalog.Application.Albums.ListAlbumsByArtist;

public sealed record ListAlbumsByArtistQuery(Guid ArtistId) : IRequest<IReadOnlyList<AlbumDto>>;
