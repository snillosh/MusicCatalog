using MusicCatalog.Application.Artists;
using MusicCatalog.Domain.Artists;

namespace MusicCatalog.Infrastructure.InMemory;

public sealed class InMemoryArtistRepository : IArtistRepository
{
    private readonly List<Artist> _artists = new();

    public Task<IReadOnlyList<Artist>> GetAllAsync(CancellationToken ct)
        => Task.FromResult((IReadOnlyList<Artist>)_artists);

    public Task<Artist?> GetByIdAsync(Guid id, CancellationToken ct)
        => Task.FromResult(_artists.FirstOrDefault(a => a.Id == id));

    public Task AddAsync(Artist artist, CancellationToken ct)
    {
        _artists.Add(artist);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsByNameAsync(string name, CancellationToken ct) =>
        Task.FromResult(_artists.Any(a => string.Equals(a.Name, name, StringComparison.OrdinalIgnoreCase)));
}
