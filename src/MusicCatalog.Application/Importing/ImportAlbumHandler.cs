using MediatR;
using MusicCatalog.Application.Common.Results;
using MusicCatalog.Contracts.Albums;
using MusicCatalog.ExternalMetadata;

namespace MusicCatalog.Application.Importing;

public class ImportAlbumHandler(
    IAlbumImportService albumImportService,
    IMusicMetadataService metadataService)
    : IRequestHandler<ImportAlbumCommand, Result<AlbumDto>>
{
    public async Task<Result<AlbumDto>> Handle(ImportAlbumCommand request, CancellationToken cancellationToken)
    {
        var albumPreview = await metadataService.LookupAlbumImportPreviewAsync(
        request.MusicBrainzReleaseGroupId,
        cancellationToken);

        if (albumPreview is null)
        {
            return Result<AlbumDto>.Fail("album.notFound", "Album not found");
        }

        var result = await albumImportService.ImportAlbumAsync(albumPreview, cancellationToken);

        return Result<AlbumDto>.Success(result);
    }
}
