using MusicCatalog.Contracts.Artists;
using MusicCatalog.Contracts.Common.Paging;

namespace MusicCatalog.ApiClient;

public interface IArtistApiClient
{
    Task<PagedResult<ArtistDto>> GetArtistsAsync(CancellationToken cancellationToken = default);
    Task<ArtistDto> CreateArtistAsync(CreateArtistRequest request, CancellationToken ct = default);
}
