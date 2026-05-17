using MediatR;
using MusicCatalog.Application.Common.Results;
using MusicCatalog.Contracts.Authentication;

namespace MusicCatalog.Application.Authentication.Login;

public sealed class LoginCommandHandler(
    IIdentityService identityService,
    IJwtTokenService jwtTokenService)
    : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    public async Task<Result<LoginResponse>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await identityService.ValidateCredentialsAsync(
        request.Email,
        request.Password,
        cancellationToken);

        if (user is null)
        {
            return Result<LoginResponse>.Fail(
            "Auth.InvalidCredentials",
            "Invalid email or password.");
        }

        var token = jwtTokenService.GenerateToken(
        user.UserId,
        user.Email,
        user.Roles);

        var response = new LoginResponse(
        token,
        DateTime.UtcNow.AddMinutes(60));

        return Result<LoginResponse>.Success(response);
    }
}
