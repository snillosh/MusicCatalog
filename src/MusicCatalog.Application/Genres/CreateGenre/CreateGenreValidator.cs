using FluentValidation;

namespace MusicCatalog.Application.Genres.CreateGenre;

public class CreateGenreValidator : AbstractValidator<CreateGenreCommand>
{
    public CreateGenreValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);
    }
}
