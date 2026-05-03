namespace MusicCatalog.Contracts.Artists;

public sealed record CreateArtistRequest(string Name, string? Country);
