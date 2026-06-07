using MediatR;
using MusicCatalog.Application.Common.Results;
using MusicCatalog.Contracts.Albums;

namespace MusicCatalog.Application.Importing;

public sealed record ImportAlbumCommand(Guid MusicBrainzReleaseGroupId) : IRequest<Result<AlbumDto>>;
