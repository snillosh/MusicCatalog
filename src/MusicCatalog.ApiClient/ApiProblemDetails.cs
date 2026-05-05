namespace MusicCatalog.ApiClient;

public sealed record ApiProblemDetails(
    string? Title,
    string? Detail,
    int? Status
);
