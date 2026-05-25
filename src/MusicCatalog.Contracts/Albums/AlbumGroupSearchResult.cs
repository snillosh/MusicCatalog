namespace MusicCatalog.Contracts.Albums;

/// <summary>
///     Search result for an album release group. This can represent multiple albums.
/// </summary>
/// <param name="ReleaseGroupId">Id of the release group.</param>
/// <param name="Title">Title of the release group.</param>
/// <param name="ArtistName">Artist Name.</param>
/// <param name="ReleaseDate">Release date.</param>
public record AlbumGroupSearchResult(
    Guid ReleaseGroupId,
    string Title,
    string ArtistName,
    string ReleaseDate);
