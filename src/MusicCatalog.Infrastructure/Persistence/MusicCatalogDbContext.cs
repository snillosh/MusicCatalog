using Microsoft.EntityFrameworkCore;
using MusicCatalog.Domain.Albums;
using MusicCatalog.Domain.Artists;

namespace MusicCatalog.Infrastructure.Persistence;

public class MusicCatalogDbContext(DbContextOptions<MusicCatalogDbContext> options) : DbContext(options)
{
    public DbSet<Artist> Artists => Set<Artist>();
    public DbSet<Album> Albums => Set<Album>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MusicCatalogDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
