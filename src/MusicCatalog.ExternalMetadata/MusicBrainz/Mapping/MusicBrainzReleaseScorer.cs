using MetaBrainz.MusicBrainz.Interfaces.Entities;

namespace MusicCatalog.ExternalMetadata.MusicBrainz.Mapping;

public static class MusicBrainzReleaseScorer
{
    public static IRelease? GetMostSuitableRelease(this IReadOnlyList<IRelease> releases)
    {
        return releases.FirstOrDefault(r => r.Media.FirstOrDefault()?.Format == "Digital Media")
               ?? releases.FirstOrDefault(r => r.Media.FirstOrDefault()?.Format == "CD")
               ?? releases.FirstOrDefault(r => r.Media.Count == 1)
               ?? releases.FirstOrDefault();
    }
}
