using MediatR;
using MusicCatalog.Contracts.Albums;

namespace MusicCatalog.Application.Metadata.GetAlbumById;

public sealed record GetAlbumMetadataByIdQuery(Guid AlbumId) : IRequest<AlbumImportPreview?>;
