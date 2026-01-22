using FluentValidation;

namespace MusicCatalog.Application.Artists.CreateArtist;

public sealed class CreateArtistValidator : AbstractValidator<CreateArtistCommand>
{
    public CreateArtistValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}
