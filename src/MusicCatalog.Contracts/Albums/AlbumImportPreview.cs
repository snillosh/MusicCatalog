using MusicCatalog.Contracts.Tracks;

namespace MusicCatalog.Contracts.Albums;

public class AlbumImportPreview
{
    public Guid MusicBrainzReleaseId { get; set; } = Guid.Empty;
    public string Title { get; set; } = string.Empty;
    public string ArtistName { get; set; } = string.Empty;
    public string ReleaseDate { get; set; } = string.Empty;
    public List<TrackPreview> Tracks { get; set; } = new();
}
