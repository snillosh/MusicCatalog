using MediatR;
using MusicCatalog.Contracts.Tracks;

namespace MusicCatalog.Application.Tracks.ListTracksByAlbum;

public sealed record ListTracksByAlbumQuery(Guid AlbumId) : IRequest<IReadOnlyList<TrackDto>>;
