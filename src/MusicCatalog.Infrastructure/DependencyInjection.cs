using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicCatalog.Application.Artists;
using MusicCatalog.Infrastructure.InMemory;

namespace MusicCatalog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IArtistRepository, InMemoryArtistRepository>();

        return services;
    }
}
