namespace MusicCatalog.Application.Albums.Dto;

public sealed record AlbumListItemDto(Guid Id, Guid ArtistId, string ArtistName, string Title, int? ReleaseYear);
