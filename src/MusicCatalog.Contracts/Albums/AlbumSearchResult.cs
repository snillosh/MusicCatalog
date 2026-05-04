namespace MusicCatalog.Contracts.Albums;

public record AlbumSearchResult(
    Guid MusicBrainzReleaseId,
    string Title,
    string ArtistName,
    string ReleaseDate,
    int TrackCount);
