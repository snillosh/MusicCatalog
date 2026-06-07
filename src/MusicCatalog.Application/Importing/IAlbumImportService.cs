using MusicCatalog.Contracts.Albums;

namespace MusicCatalog.Application.Importing;

public interface IAlbumImportService
{
    Task<AlbumDto> ImportAlbumAsync(
        AlbumImportPreview selectedAlbum,
        CancellationToken cancellationToken = default);
}
