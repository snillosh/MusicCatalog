namespace MusicCatalog.Contracts.Albums;

public sealed record UpdateAlbumRequest(string Title, int? ReleaseYear);
