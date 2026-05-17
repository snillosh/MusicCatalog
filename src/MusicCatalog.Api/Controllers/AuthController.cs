using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicCatalog.Application.Authentication.Login;
using MusicCatalog.Application.Authentication.Register;
using MusicCatalog.Contracts.Authentication;

namespace MusicCatalog.Api.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(ISender sender) : ControllerBase
{
    [AllowAnonymous]
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

    [AllowAnonymous]
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

    [Authorize]
    [HttpGet("me")]
    public IActionResult Me()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                     ?? User.FindFirstValue("sub");

        var email = User.FindFirstValue(ClaimTypes.Email)
                    ?? User.FindFirstValue("email");

        var roles = User.FindAll(ClaimTypes.Role)
            .Select(c => c.Value)
            .ToArray();

        return Ok(
        new
        {
            UserId = userId, Email = email, Roles = roles
        });
    }
}
