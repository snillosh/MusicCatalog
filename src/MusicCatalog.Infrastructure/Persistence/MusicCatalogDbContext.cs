using Microsoft.EntityFrameworkCore;
using MusicCatalog.Domain.Albums;
using MusicCatalog.Domain.Artists;
using MusicCatalog.Domain.Genre;
using MusicCatalog.Domain.Tracks;

namespace MusicCatalog.Infrastructure.Persistence;

public class MusicCatalogDbContext(DbContextOptions<MusicCatalogDbContext> options) : DbContext(options)
{
    public DbSet<Artist> Artists => Set<Artist>();
    public DbSet<Album> Albums => Set<Album>();
    public DbSet<Track> Tracks => Set<Track>();
    public DbSet<Genre> Genres => Set<Genre>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MusicCatalogDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
