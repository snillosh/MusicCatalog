namespace MusicCatalog.Contracts.Albums;

public record AlbumSearchResult(
    Guid ReleaseGroupId,
    Guid ReleaseId,
    string Title,
    string ArtistName,
    string ReleaseDate,
    int TrackCount);
