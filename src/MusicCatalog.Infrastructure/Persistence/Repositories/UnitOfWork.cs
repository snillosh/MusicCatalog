using MusicCatalog.Application.Saving;

namespace MusicCatalog.Infrastructure.Persistence.Repositories;

public sealed class UnitOfWork(MusicCatalogDbContext db)
    : IUnitOfWork
{
    public Task SaveChangesAsync(CancellationToken ct)
        => db.SaveChangesAsync(ct);
}
