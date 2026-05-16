using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicCatalog.Application.Albums;
using MusicCatalog.Application.Artists;
using MusicCatalog.Application.Genres;
using MusicCatalog.Application.Tracks;
using MusicCatalog.Infrastructure.Identity;
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
        
        services
            .AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.User.RequireUniqueEmail = true;

                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddEntityFrameworkStores<MusicCatalogDbContext>()
            .AddDefaultTokenProviders();
        
        services
            .AddAuthentication()
            .AddJwtBearer();

        services.AddScoped<IArtistRepository, ArtistRepository>();
        services.AddScoped<IAlbumRepository, AlbumRepository>();
        services.AddScoped<ITrackRepository, TrackRepository>();
        services.AddScoped<IGenreRepository, GenreRepository>();

        return services;
    }
}
