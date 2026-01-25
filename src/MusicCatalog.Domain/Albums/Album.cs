using MusicCatalog.Domain.Artists;
using MusicCatalog.Domain.Tracks;

namespace MusicCatalog.Domain.Albums;

public sealed class Album
{
    private Album()
    {
    }

    public Album(Guid artistId, string title, int? releaseYear = null)
    {
        Id = Guid.NewGuid();
        ArtistId = artistId;
        Rename(title);
        SetReleaseYear(releaseYear);
    }

    public Guid Id { get; private set; }

    public Guid ArtistId { get; private set; }

    public Artist Artist { get; private set; } = default!;

    public ICollection<Track> Tracks { get; private set; } = new List<Track>();

    public string Title { get; private set; } = default!;

    public int? ReleaseYear { get; private set; }

    public void Rename(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Album title cannot be empty.", nameof(title));

        Title = title.Trim();
    }

    public void SetReleaseYear(int? year)
    {
        if (year is null)
        {
            ReleaseYear = null;
            return;
        }

        if (year < 1880 || year > DateTime.UtcNow.Year + 1)
            throw new ArgumentOutOfRangeException(nameof(year), "Release year is out of range.");

        ReleaseYear = year;
    }
}
