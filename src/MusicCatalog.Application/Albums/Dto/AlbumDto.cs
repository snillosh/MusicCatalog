namespace MusicCatalog.Application.Albums.Dto;

public sealed record AlbumDto(Guid Id, Guid ArtistId, string Title, int? ReleaseYear);
