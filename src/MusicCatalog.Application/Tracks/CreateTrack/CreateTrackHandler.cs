using MediatR;
using MusicCatalog.Application.Albums;
using MusicCatalog.Application.Common.Results;
using MusicCatalog.Contracts.Tracks;
using MusicCatalog.Domain.Tracks;

namespace MusicCatalog.Application.Tracks.CreateTrack;

public sealed class CreateTrackHandler(IAlbumRepository albums, ITrackRepository tracks)
    : IRequestHandler<CreateTrackCommand, Result<TrackDto>>
{
    public async Task<Result<TrackDto>> Handle(CreateTrackCommand request, CancellationToken ct)
    {
        var album = await albums.GetByIdAsync(request.AlbumId, ct);
        if (album is null)
            return Result<TrackDto>.Fail("albums.notFound", "Album not found.");

        if (await tracks.ExistsTrackNumberAsync(request.AlbumId, request.TrackNumber, ct))
            return Result<TrackDto>.Fail("tracks.duplicateTrackNumber",
            "That track number already exists for this album.");

        var title = request.Title.Trim();

        var track = new Track(request.AlbumId, request.TrackNumber, title, request.DurationSeconds);

        await tracks.AddAsync(track, ct);

        return Result<TrackDto>.Success(
        new TrackDto(track.Id, track.AlbumId, track.TrackNumber, track.Title, track.DurationSeconds));
    }
}
