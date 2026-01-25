using MediatR;
using MusicCatalog.Application.Common.Results;
using MusicCatalog.Application.Tracks.Dto;

namespace MusicCatalog.Application.Tracks.CreateTrack;

public sealed record CreateTrackCommand(
    Guid AlbumId,
    int TrackNumber,
    string Title,
    int? DurationSeconds
) : IRequest<Result<TrackDto>>;
