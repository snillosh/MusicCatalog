using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicCatalog.Application.Artists;
using MusicCatalog.Infrastructure.Persistence;
using MusicCatalog.Infrastructure.Persistence.Repositories;

namespace MusicCatalog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MusicCatalog")
                               ?? throw new InvalidOperationException("Connection string 'MusicCatalog' not found.");

        services.AddDbContext<MusicCatalogDbContext>(options => { options.UseNpgsql(connectionString); });

        services.AddScoped<IArtistRepository, ArtistRepository>();

        return services;
    }
}
