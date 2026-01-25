using MediatR;
using MusicCatalog.Application.Tracks.Dto;

namespace MusicCatalog.Application.Tracks.ListTracksByAlbum;

public class ListTracksByAlbumHandler(ITrackRepository repository)
    : IRequestHandler<ListTracksByAlbumQuery, IReadOnlyList<TrackDto>>
{
    public async Task<IReadOnlyList<TrackDto>> Handle(ListTracksByAlbumQuery request,
        CancellationToken cancellationToken)
    {
        var tracks = await repository.GetByAlbumIdAsync(request.AlbumId, cancellationToken);

        return tracks.Select(x => new TrackDto(x.Id, x.AlbumId, x.TrackNumber, x.Title, x.DurationSeconds)).ToList();
    }
}
