using MediatR;
using MusicCatalog.Contracts.Albums;
using MusicCatalog.ExternalMetadata;

namespace MusicCatalog.Application.Metadata.GetAlbumById;

public class GetAlbumMetadataByIdHandler(IMusicMetadataService metadataService)
    : IRequestHandler<GetAlbumMetadataByIdQuery, AlbumImportPreview?>
{
    public async Task<AlbumImportPreview?> Handle(
        GetAlbumMetadataByIdQuery request,
        CancellationToken cancellationToken)
    {
        var album = await metadataService.LookupAlbumImportPreviewAsync(request.AlbumId, cancellationToken);
        return album;
    }
}
