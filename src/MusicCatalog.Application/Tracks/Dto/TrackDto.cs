namespace MusicCatalog.Application.Tracks.Dto;

public sealed record TrackDto(Guid Id, Guid AlbumId, int TrackNumber, string Title, int? DurationSeconds);
