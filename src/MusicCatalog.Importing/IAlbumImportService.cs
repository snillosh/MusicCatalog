using MusicCatalog.Contracts.Albums;

namespace MusicCatalog.Importing;

public interface IAlbumImportService
{
    Task ImportAlbumAsync(AlbumImportPreview selectedAlbum, CancellationToken cancellationToken = default);
}
