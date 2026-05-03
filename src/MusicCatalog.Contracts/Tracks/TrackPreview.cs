namespace MusicCatalog.Contracts.Tracks;

public class TrackPreview
{
    public string Title { get; set; } = string.Empty;
    public int TrackNumber { get; set; }
    public int? DurationSeconds { get; set; } = 0;
}
