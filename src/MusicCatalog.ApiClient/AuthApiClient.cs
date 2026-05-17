using System.Net.Http.Json;
using MusicCatalog.Contracts.Authentication;

namespace MusicCatalog.ApiClient;

public class AuthApiClient(IHttpClientFactory httpClientFactory) : IAuthApiClient
{
    private readonly HttpClient _http = httpClientFactory.CreateClient("MusicCatalogApi");

    public async Task<LoginResponse> LoginAsync(
        LoginRequest request,
        CancellationToken ct = default)
    {
        var response = await _http.PostAsJsonAsync(
        "api/auth/login",
        request,
        ct);

        if (!response.IsSuccessStatusCode)
        {
            var message = "Login failed.";

            if (response.Content.Headers.ContentLength > 0)
            {
                var problem = await response.Content.ReadFromJsonAsync<ApiProblemDetails>(ct);
                message = problem?.Detail ?? problem?.Title ?? message;
            }

            throw new HttpRequestException(message, null, response.StatusCode);
        }

        var result = await response.Content.ReadFromJsonAsync<LoginResponse>(ct);

        return result
               ?? throw new InvalidOperationException("API returned an empty login response.");
    }

    public async Task RegisterAsync(
        RegisterRequest request,
        CancellationToken ct = default)
    {
        var response = await _http.PostAsJsonAsync(
        "api/auth/register",
        request,
        ct);

        if (!response.IsSuccessStatusCode)
        {
            var message = "Registration failed.";

            if (response.Content.Headers.ContentLength > 0)
            {
                var problem = await response.Content.ReadFromJsonAsync<ApiProblemDetails>(ct);
                message = problem?.Detail ?? problem?.Title ?? message;
            }

            throw new HttpRequestException(message, null, response.StatusCode);
        }
    }
}
