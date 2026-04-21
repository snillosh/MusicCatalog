using MediatR;
using MusicCatalog.Application.Genres.Dto;

namespace MusicCatalog.Application.Genres.GetGenreById;

public sealed record GetGenreByIdQuery(Guid Id) : IRequest<GenreDto?>;
