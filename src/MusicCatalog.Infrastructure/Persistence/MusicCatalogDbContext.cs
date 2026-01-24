using Microsoft.EntityFrameworkCore;
using MusicCatalog.Domain.Artists;

namespace MusicCatalog.Infrastructure.Persistence;

public class MusicCatalogDbContext(DbContextOptions<MusicCatalogDbContext> options) : DbContext(options)
{
    public DbSet<Artist> Artists => Set<Artist>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MusicCatalogDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
