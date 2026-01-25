using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicCatalog.Domain.Albums;
using MusicCatalog.Domain.Artists;

namespace MusicCatalog.Infrastructure.Persistence.Configurations;

public sealed class AlbumConfiguration : IEntityTypeConfiguration<Album>
{
    public void Configure(EntityTypeBuilder<Album> builder)
    {
        builder.ToTable(nameof(Album).ToLowerInvariant());

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).HasMaxLength(200).IsRequired();

        builder.Property(x => x.ReleaseYear).IsRequired(false);

        builder.Property(x => x.ArtistId).IsRequired();

        builder.HasOne<Artist>(a => a.Artist).WithMany(a => a.Albums).HasForeignKey(x => x.ArtistId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.ArtistId, x.Title }).IsUnique();
    }
}
