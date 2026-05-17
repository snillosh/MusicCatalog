using MediatR;
using MusicCatalog.Application.Common.Results;

namespace MusicCatalog.Application.Authentication.Register;

public sealed record RegisterCommand(
    string Email,
    string Password,
    string? DisplayName) : IRequest<Result>;
