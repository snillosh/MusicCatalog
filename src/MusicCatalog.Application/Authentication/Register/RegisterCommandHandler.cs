using MediatR;

namespace MusicCatalog.Application.Authentication.Register;

public sealed class RegisterCommandHandler(IIdentityService identityService)
    : IRequestHandler<RegisterCommand, bool>
{
    public Task<bool> Handle(
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
