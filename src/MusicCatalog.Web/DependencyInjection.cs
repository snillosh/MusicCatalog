using MusicCatalog.ApiClient;
using MusicCatalog.ExternalMetadata;
using MusicCatalog.ExternalMetadata.MusicBrainz.Metadata;
using MusicCatalog.Importing;
using MusicCatalog.Web.Auth;

namespace MusicCatalog.Web;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddWeb()
        {
            services.AddRazorComponents()
                .AddInteractiveServerComponents();

            return services;
        }

        public IServiceCollection AddApiClients(IConfiguration config)
        {
            services.AddHttpClient(
            "MusicCatalogApi",
            c => { c.BaseAddress = new Uri(config["MusicCatalogApi:BaseUrl"]!); });

            services.AddScoped<IAlbumApiClient, AlbumApiClient>();
            services.AddScoped<IArtistApiClient, ArtistApiClient>();
            services.AddScoped<ITrackApiClient, TrackApiClient>();
            services.AddScoped<IAuthApiClient, AuthApiClient>();

            return services;
        }

        public IServiceCollection AddAuthenticationServices()
        {
            services.AddScoped<IAccessTokenStore, ServerAccessTokenStore>();

            return services;
        }

        public IServiceCollection AddApplicationServices()
        {
            services.AddScoped<IAlbumImportService, AlbumImportService>();

            return services;
        }

        public IServiceCollection AddExternalServices()
        {
            services.AddScoped<IMusicMetadataService, MusicBrainzMetadataService>();
            return services;
        }
    }
}
