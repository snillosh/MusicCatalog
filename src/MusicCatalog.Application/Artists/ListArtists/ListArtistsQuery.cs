using MediatR;
using MusicCatalog.Contracts.Artists;
using MusicCatalog.Contracts.Common.Paging;

namespace MusicCatalog.Application.Artists.ListArtists;

public sealed record ListArtistsQuery(int Page = 1, int PageSize = 50) : IRequest<PagedResult<ArtistDto>>;
