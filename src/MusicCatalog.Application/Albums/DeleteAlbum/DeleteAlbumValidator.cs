using FluentValidation;

namespace MusicCatalog.Application.Albums.DeleteAlbum;

public class DeleteAlbumValidator : AbstractValidator<DeleteAlbumCommand>
{
    public DeleteAlbumValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
