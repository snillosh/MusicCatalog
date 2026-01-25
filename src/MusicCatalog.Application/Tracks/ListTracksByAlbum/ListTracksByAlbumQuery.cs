using MediatR;
using MusicCatalog.Application.Tracks.Dto;

namespace MusicCatalog.Application.Tracks.ListTracksByAlbum;

public sealed record ListTracksByAlbumQuery(Guid AlbumId) : IRequest<IReadOnlyList<TrackDto>>;
