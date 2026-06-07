using MusicCatalog.Contracts.Tracks;

namespace MusicCatalog.Contracts.Albums;

public record AlbumImportPreview(
    Guid MusicBrainzReleaseGroupId,
    Guid MusicBrainzReleaseId,
    string Title,
    string ArtistName,
    string ReleaseDate,
    List<TrackPreview> Tracks);
