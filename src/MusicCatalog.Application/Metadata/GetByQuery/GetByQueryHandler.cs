using MediatR;
using MusicCatalog.Contracts.Albums;
using MusicCatalog.ExternalMetadata;

namespace MusicCatalog.Application.Metadata.GetByQuery;

public class GetByQueryHandler(IMusicMetadataService metadataService)
    : IRequestHandler<GetByQueryQuery, IReadOnlyList<AlbumGroupSearchResult>>
{
    public async Task<IReadOnlyList<AlbumGroupSearchResult>> Handle(GetByQueryQuery request, CancellationToken ct)
    {
        var results = await metadataService.FindSimpleAlbumGroupsAsync(request.Query, ct);
        return results;
    }
}
