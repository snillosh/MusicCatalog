using MusicCatalog.ApiClient;
using MusicCatalog.ExternalMetadata;
using MusicCatalog.Importing;

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
            services.AddHttpClient<IMusicCatalogApiClient, MusicCatalogApiClient>(c =>
            {
                c.BaseAddress = new Uri(config["MusicCatalogApi:BaseUrl"]!);
            });

            return services;
        }

        public IServiceCollection AddApplicationServices()
        {
            services.AddSingleton<IAlbumImportService, AlbumImportService>();
            return services;
        }

        public IServiceCollection AddExternalServices()
        {
            services.AddSingleton<IMusicMetadataService, MusicBrainzMetadataService>();
            return services;
        }
    }
}
