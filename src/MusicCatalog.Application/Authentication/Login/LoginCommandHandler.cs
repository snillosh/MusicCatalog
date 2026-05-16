using MediatR;
using MusicCatalog.Contracts.Authentication;

namespace MusicCatalog.Application.Authentication.Login;

public sealed class LoginCommandHandler(
    IIdentityService identityService,
    IJwtTokenService jwtTokenService)
    : IRequestHandler<LoginCommand, LoginResponse?>
{
    public async Task<LoginResponse?> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await identityService.ValidateCredentialsAsync(
            request.Email,
            request.Password,
            cancellationToken);

        if (user is null)
        {
            return null;
        }

        var token = jwtTokenService.GenerateToken(
            user.UserId,
            user.Email,
            user.Roles);

        return new LoginResponse(
            token,
            DateTime.UtcNow.AddMinutes(60));
    }
}
