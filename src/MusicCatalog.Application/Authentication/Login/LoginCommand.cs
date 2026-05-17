using MediatR;
using MusicCatalog.Application.Common.Results;
using MusicCatalog.Contracts.Authentication;

namespace MusicCatalog.Application.Authentication.Login;

public sealed record LoginCommand(
    string Email,
    string Password) : IRequest<Result<LoginResponse>>;
