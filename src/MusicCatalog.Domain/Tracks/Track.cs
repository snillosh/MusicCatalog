using MusicCatalog.Domain.Albums;

namespace MusicCatalog.Domain.Tracks;

public sealed class Track
{
    private Track() {}

    public Track(Guid albumId, int trackNumber, string title, int? durationSeconds)
    {
        Id = Guid.NewGuid();
        AlbumId = albumId;
        SetTrackNumber(trackNumber);
        Rename(title);
        SetDurationSeconds(durationSeconds);
    }

    public Guid Id { get; private set; }

    public Guid AlbumId { get; private set; }

    public Album Album { get; private set; } = default!;

    public int TrackNumber { get; private set; }
    public string Title { get; private set; } = default!;
    public int? DurationSeconds { get; private set; }

    public void Rename(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentNullException("Track title cannot be empty", nameof(title));
        }

        Title = title.Trim();
    }

    public void SetTrackNumber(int trackNumber)
    {
        if (trackNumber < 1 || trackNumber > 200)
        {
            throw new ArgumentOutOfRangeException(nameof(trackNumber), "Track number must be between 1 and 200");
        }

        TrackNumber = trackNumber;
    }

    public void SetDurationSeconds(int? durationSeconds)
    {
        if (durationSeconds is null)
        {
            return;
        }

        if (durationSeconds < 0 || durationSeconds > 60 * 60 * 5)
        {
            throw new ArgumentOutOfRangeException(
            nameof(durationSeconds),
            "Duration seconds must be between 0 seconds and 5 hours");
        }

        DurationSeconds = durationSeconds;
    }
}
