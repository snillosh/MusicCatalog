using MediatR;

namespace MusicCatalog.Application.Authentication.Register;

public sealed record RegisterCommand(
    string Email,
    string Password,
    string? DisplayName) : IRequest<bool>;
