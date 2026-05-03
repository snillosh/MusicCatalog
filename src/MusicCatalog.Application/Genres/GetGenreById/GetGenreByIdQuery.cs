using MediatR;
using MusicCatalog.Contracts.Genres;

namespace MusicCatalog.Application.Genres.GetGenreById;

public sealed record GetGenreByIdQuery(Guid Id) : IRequest<GenreDto?>;
