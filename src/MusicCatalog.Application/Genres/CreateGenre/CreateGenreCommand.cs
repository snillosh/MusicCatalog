using MediatR;
using MusicCatalog.Application.Common.Results;
using MusicCatalog.Application.Genres.Dto;

namespace MusicCatalog.Application.Genres.CreateGenre;

public sealed record CreateGenreCommand(string Title) : IRequest<Result<GenreDto?>>;
