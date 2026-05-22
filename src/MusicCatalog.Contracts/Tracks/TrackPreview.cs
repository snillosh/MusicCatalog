namespace MusicCatalog.Contracts.Tracks;

public record TrackPreview(string Title, int TrackNumber, int? DurationSeconds)
{
    public override string ToString() => $"{TrackNumber}.  {Title}.  {DurationSeconds}s";
}
