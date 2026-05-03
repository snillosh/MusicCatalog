using MediatR;
using MusicCatalog.Application.Common.Results;
using MusicCatalog.Contracts.Genres;

namespace MusicCatalog.Application.Genres.CreateGenre;

public sealed record CreateGenreCommand(string Title) : IRequest<Result<GenreDto?>>;
