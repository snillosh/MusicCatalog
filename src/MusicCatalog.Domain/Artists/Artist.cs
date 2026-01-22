namespace MusicCatalog.Domain.Artists;

public sealed class Artist
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public required string Name { get; init; }
}
