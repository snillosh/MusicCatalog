using MediatR;
using MusicCatalog.Application.Common.Results;
using MusicCatalog.Contracts.Artists;

namespace MusicCatalog.Application.Artists.CreateArtist;

public record CreateArtistCommand(string Name, string? Country) : IRequest<Result<ArtistDto>>;
