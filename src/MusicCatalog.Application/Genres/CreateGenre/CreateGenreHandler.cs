using MediatR;
using MusicCatalog.Application.Common.Results;
using MusicCatalog.Contracts.Genres;
using MusicCatalog.Domain.Genre;

namespace MusicCatalog.Application.Genres.CreateGenre;

public sealed class CreateGenreHandler(IGenreRepository repo) : IRequestHandler<CreateGenreCommand, Result<GenreDto>>
{
    public async Task<Result<GenreDto>> Handle(CreateGenreCommand request, CancellationToken cancellationToken)
    {
        var title = request.Title.Trim();

        var genre = new Genre(title);

        if (await repo.ExistsWithTitleAsync(title, genre.Id, cancellationToken))
        {
            return Result<GenreDto>.Fail("genres.duplicate", "There is already a genre with that title.");
        }

        await repo.AddAsync(genre, cancellationToken);

        return Result<GenreDto>.Success(new GenreDto(genre.Id, genre.Title));
    }
}
