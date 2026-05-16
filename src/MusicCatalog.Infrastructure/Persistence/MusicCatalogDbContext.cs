using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MusicCatalog.Domain.Albums;
using MusicCatalog.Domain.Artists;
using MusicCatalog.Domain.Genre;
using MusicCatalog.Domain.Tracks;
using MusicCatalog.Infrastructure.Identity;

namespace MusicCatalog.Infrastructure.Persistence;

public class MusicCatalogDbContext(DbContextOptions<MusicCatalogDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Artist> Artists => Set<Artist>();
    public DbSet<Album> Albums => Set<Album>();
    public DbSet<Track> Tracks => Set<Track>();
    public DbSet<Genre> Genres => Set<Genre>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(MusicCatalogDbContext).Assembly);
    }
}
