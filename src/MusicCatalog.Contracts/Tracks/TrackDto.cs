namespace MusicCatalog.Contracts.Tracks;

public sealed record TrackDto(Guid Id, Guid AlbumId, int TrackNumber, string Title, int? DurationSeconds);
