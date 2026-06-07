namespace MusicCatalog.Contracts.Albums;

public sealed record CreateAlbumRequest(
    Guid musicBrainzReleaseGroupId,
    Guid musicBrainzReleaseId,
    string Title,
    int? ReleaseYear);
