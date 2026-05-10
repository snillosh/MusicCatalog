namespace MusicCatalog.Domain.Genre;

public sealed class Genre
{
    private Genre() {}

    public Genre(string title)
    {
        Id = Guid.NewGuid();
        Rename(title);
    }

    public Guid Id { get; private set; }

    public string Title { get; private set; } = default!;

    public void Rename(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException("Album title cannot be empty.", nameof(title));
        }

        Title = title.Trim();
    }
}
