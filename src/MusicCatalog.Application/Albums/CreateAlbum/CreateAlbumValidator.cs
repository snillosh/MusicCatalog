using FluentValidation;

namespace MusicCatalog.Application.Albums.CreateAlbum;

public sealed class CreateAlbumValidator : AbstractValidator<CreateAlbumCommand>
{
    public CreateAlbumValidator()
    {
        RuleFor(x => x.ArtistId).NotEmpty();

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.ReleaseYear)
            .InclusiveBetween(1880, DateTime.UtcNow.Year + 1)
            .When(x => x.ReleaseYear is not null);
    }
}
