using Microsoft.Extensions.DependencyInjection;
using MusicCatalog.ExternalMetadata.MusicBrainz.Metadata;

namespace MusicCatalog.ExternalMetadata;

public static class DependencyInjection
{
    public static IServiceCollection AddMetadata(this IServiceCollection services)
    {
        services.AddScoped<IMusicMetadataService, MusicBrainzMetadataService>();

        return services;
    }
}
