namespace MusicCatalog.Application.Saving;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken ct);
}
