using MediatR;
using MusicCatalog.Application.Artists.Dto;
using MusicCatalog.Application.Common.Results;

namespace MusicCatalog.Application.Artists.CreateArtist;

public record CreateArtistCommand(string Name, string? Country) : IRequest<Result<ArtistDto>>;
