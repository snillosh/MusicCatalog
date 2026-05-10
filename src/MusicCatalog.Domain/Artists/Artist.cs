using MusicCatalog.Domain.Albums;

namespace MusicCatalog.Domain.Artists;

public sealed class Artist
{
    private Artist() {}

    public Artist(string name, string? country = null)
    {
        Id = Guid.NewGuid();
        Name = name;
        Country = country;
    }

    public ICollection<Album> Albums { get; private set; } = new List<Album>();

    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public string? Country { get; private set; }

    public void Rename(string name) => Name = name;

    public void SetCountry(string? country)
    {
        if (country == null)
        {
            Country = null;
            return;
        }

        if (country.Length != 2)
        {
            throw new ArgumentException("Country must be a 2 letter code");
        }

        Country = country.ToUpperInvariant();
    }
}
