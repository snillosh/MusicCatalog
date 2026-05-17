using MediatR;
using MusicCatalog.Application.Common.Results;

namespace MusicCatalog.Application.Authentication.Register;

public sealed class RegisterCommandHandler(IIdentityService identityService)
    : IRequestHandler<RegisterCommand, Result>
{
    public Task<Result> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        return identityService.CreateUserAsync(
        request.Email,
        request.Password,
        request.DisplayName,
        cancellationToken);
    }
}
