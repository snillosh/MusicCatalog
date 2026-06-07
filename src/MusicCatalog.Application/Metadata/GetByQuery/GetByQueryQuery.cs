using MediatR;
using MusicCatalog.Contracts.Albums;

namespace MusicCatalog.Application.Metadata.GetByQuery;

public sealed record GetByQueryQuery(string Query) : IRequest<IReadOnlyList<AlbumGroupSearchResult>>;
