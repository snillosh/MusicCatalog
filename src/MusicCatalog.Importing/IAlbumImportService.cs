using MusicCatalog.Contracts.Albums;

namespace MusicCatalog.Importing;

public interface IAlbumImportService
{
    Task<AlbumDto> ImportAlbumAsync(AlbumImportPreview selectedAlbum, CancellationToken cancellationToken = default);
}
