namespace MusicCatalog.Contracts.Tracks;

public sealed record CreateTrackRequest(int TrackNumber, string Title, int? DurationSeconds);
