namespace MusicCatalog.Contracts.Albums;

public sealed record AlbumDto(Guid Id, Guid ArtistId, string Title, int? ReleaseYear);
