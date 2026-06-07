namespace MusicCatalog.Contracts.Albums;

public sealed record AlbumListItemDto(
    Guid Id,
    Guid ArtistId,
    Guid musicBrainzReleaseGroupId,
    Guid musicBrainzReleaseId,
    string ArtistName,
    string Title,
    int? ReleaseYear);
