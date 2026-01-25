using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicCatalog.Domain.Artists;

namespace MusicCatalog.Infrastructure.Persistence.Configurations;

public sealed class ArtistConfiguration : IEntityTypeConfiguration<Artist>
{
    public void Configure(EntityTypeBuilder<Artist> builder)
    {
        builder.ToTable(nameof(Artist));

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();

        builder.Property(x => x.Country).HasMaxLength(2).IsUnicode(false).IsRequired(false);

        builder.HasIndex(x => x.Name).IsUnique();
    }
}
