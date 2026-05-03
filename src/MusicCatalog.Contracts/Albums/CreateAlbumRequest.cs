namespace MusicCatalog.Contracts.Albums;

public sealed record CreateAlbumRequest(string Title, int? ReleaseYear);
