using FluentValidation;

namespace MusicCatalog.Application.Albums.ListAlbums;

public sealed class ListAlbumsValidator : AbstractValidator<ListAlbumsQuery>
{
    public ListAlbumsValidator()
    {
        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .LessThanOrEqualTo(200);

        RuleFor(x => x.ReleasedAfter)
            .InclusiveBetween(1880, DateTime.UtcNow.Year + 1)
            .When(x => x.ReleasedAfter is not null);
    }
}
