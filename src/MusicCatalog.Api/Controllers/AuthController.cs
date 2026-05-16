using MediatR;
using Microsoft.AspNetCore.Mvc;
using MusicCatalog.Application.Authentication.Login;
using MusicCatalog.Application.Authentication.Register;
using MusicCatalog.Contracts.Authentication;

namespace MusicCatalog.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(ISender sender) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(
        [FromBody] LoginRequest request,
        CancellationToken ct)
    {
        var response = await sender.Send(
            new LoginCommand(request.Email, request.Password),
            ct);

        return response is null ? Unauthorized() : Ok(response);
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest request,
        CancellationToken ct)
    {
        var created = await sender.Send(
            new RegisterCommand(request.Email, request.Password, request.DisplayName),
            ct);

        return created ? Created() : BadRequest();
    }
}
