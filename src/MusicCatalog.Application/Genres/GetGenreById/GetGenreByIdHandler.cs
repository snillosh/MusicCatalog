using MediatR;
using MusicCatalog.Contracts.Genres;

namespace MusicCatalog.Application.Genres.GetGenreById;

public class GetGenreByIdHandler(IGenreRepository repo) : IRequestHandler<GetGenreByIdQuery, GenreDto?>
{
    public async Task<GenreDto?> Handle(GetGenreByIdQuery request, CancellationToken cancellationToken)
    {
        var genre = await repo.GetByIdAsync(request.Id, cancellationToken);
        return genre is null ? null : new GenreDto(genre.Id, genre.Title);
    }
}
