using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicCatalog.Domain.Albums;
using MusicCatalog.Domain.Tracks;

namespace MusicCatalog.Infrastructure.Persistence.Configurations;

public class TrackConfiguration : IEntityTypeConfiguration<Track>
{
    public void Configure(EntityTypeBuilder<Track> builder)
    {
        builder.ToTable(nameof(Track).ToLowerInvariant());

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).HasMaxLength(200).IsRequired();

        builder.Property(x => x.TrackNumber).IsRequired();

        builder.Property(x => x.DurationSeconds).IsRequired(false);

        builder.Property(x => x.AlbumId).IsRequired();

        builder.HasOne<Album>(x => x.Album).WithMany(x => x.Tracks).HasForeignKey(x => x.AlbumId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new { x.AlbumId, x.TrackNumber }).IsUnique();
    }
}
