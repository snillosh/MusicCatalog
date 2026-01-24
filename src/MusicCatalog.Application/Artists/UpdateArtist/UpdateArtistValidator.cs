using FluentValidation;

namespace MusicCatalog.Application.Artists.UpdateArtist;

public sealed class UpdateArtistValidator : AbstractValidator<UpdateArtistCommand>
{
    public UpdateArtistValidator()
    {
        RuleFor(x => x.Id).NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);
    }
}
