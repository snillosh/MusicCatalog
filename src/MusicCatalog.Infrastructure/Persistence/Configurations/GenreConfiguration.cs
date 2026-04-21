using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicCatalog.Domain.Genre;

namespace MusicCatalog.Infrastructure.Persistence.Configurations;

public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.ToTable(nameof(Genre).ToLowerInvariant());
        
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).HasMaxLength(200).IsRequired();
        
        builder.HasIndex(x => new { x.Title }).IsUnique();
    }
}
