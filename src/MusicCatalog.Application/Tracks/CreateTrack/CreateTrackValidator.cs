using FluentValidation;

namespace MusicCatalog.Application.Tracks.CreateTrack;

public sealed class CreateTrackValidator : AbstractValidator<CreateTrackCommand>
{
    public CreateTrackValidator()
    {
        RuleFor(x => x.AlbumId).NotEmpty();

        RuleFor(x => x.TrackNumber)
            .InclusiveBetween(1, 200);

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.DurationSeconds)
            .InclusiveBetween(0, 60 * 60 * 5)
            .When(x => x.DurationSeconds is not null);
    }
}
