using FluentValidation;

namespace MusicCatalog.Application.Artists.DeleteArtist;

public sealed class DeleteArtistValidator : AbstractValidator<DeleteArtistCommand>
{
    public DeleteArtistValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
