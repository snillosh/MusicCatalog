using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MusicCatalog.Domain.Artists;
using MusicCatalog.Infrastructure.Persistence;
using MusicCatalog.Infrastructure.Persistence.Repositories;
using NUnit.Framework;
using Testcontainers.PostgreSql;

namespace MusicCatalog.Infrastructure.Tests;

[TestFixture]
public class ArtistRepositoryTests
{

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        _postgres = new PostgreSqlBuilder("postgres:16")
            .WithDatabase("musiccatalog_tests")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

        await _postgres.StartAsync();
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown() => await _postgres.DisposeAsync();

    [SetUp]
    public async Task SetUp()
    {
        var options = new DbContextOptionsBuilder<MusicCatalogDbContext>()
            .UseNpgsql(_postgres.GetConnectionString())
            .Options;

        _db = new MusicCatalogDbContext(options);

        await _db.Database.EnsureDeletedAsync();
        await _db.Database.EnsureCreatedAsync();

        _repository = new ArtistRepository(_db);
    }

    [TearDown]
    public async Task TearDown() => await _db.DisposeAsync();
    private PostgreSqlContainer _postgres = null!;
    private MusicCatalogDbContext _db = null!;
    private ArtistRepository _repository = null!;

    [Test]
    public async Task AddAsync_SavesArtist()
    {
        var artist = new Artist("Magdalena Bay", "US");

        await _repository.AddAsync(artist, CancellationToken.None);

        var savedArtist = await _db.Artists.FirstOrDefaultAsync(x => x.Id == artist.Id);

        savedArtist.Should().NotBeNull();
        savedArtist!.Name.Should().Be("Magdalena Bay");
        savedArtist.Country.Should().Be("US");
    }

    [Test]
    public async Task GetByIdAsync_WhenArtistExists_ReturnsArtist()
    {
        var artist = new Artist("Magdalena Bay", "US");

        await _db.Artists.AddAsync(artist);
        await _db.SaveChangesAsync();

        var result = await _repository.GetByIdAsync(artist.Id, CancellationToken.None);

        result.Should().NotBeNull();
        result!.Id.Should().Be(artist.Id);
        result.Name.Should().Be("Magdalena Bay");
        result.Country.Should().Be("US");
    }

    [Test]
    public async Task GetByIdAsync_WhenArtistDoesNotExist_ReturnsNull()
    {
        var result = await _repository.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

        result.Should().BeNull();
    }

    [Test]
    public async Task GetByIdAsync_ReturnsUntrackedEntity()
    {
        var artist = new Artist("Magdalena Bay", "US");

        await _db.Artists.AddAsync(artist);
        await _db.SaveChangesAsync();

        _db.ChangeTracker.Clear();

        var result = await _repository.GetByIdAsync(artist.Id, CancellationToken.None);

        result.Should().NotBeNull();

        var tracked = _db.ChangeTracker
            .Entries<Artist>()
            .Any(x => x.Entity.Id == artist.Id && x.State != EntityState.Detached);

        tracked.Should().BeFalse();
    }

    [Test]
    public async Task GetByIdTrackedAsync_ReturnsTrackedEntity()
    {
        var artist = new Artist("Magdalena Bay", "US");

        await _db.Artists.AddAsync(artist);
        await _db.SaveChangesAsync();

        _db.ChangeTracker.Clear();

        var result = await _repository.GetByIdTrackedAsync(artist.Id, CancellationToken.None);

        result.Should().NotBeNull();

        var tracked = _db.ChangeTracker
            .Entries<Artist>()
            .Any(x => x.Entity.Id == artist.Id && x.State != EntityState.Detached);

        tracked.Should().BeTrue();
    }

    [Test]
    public async Task GetAllAsync_ReturnsPagedArtistsOrderedByName()
    {
        var artist1 = new Artist("Magdalena Bay", "US");
        var artist2 = new Artist("Cocteau Twins", "UK");
        var artist3 = new Artist("Beach House", "US");

        await _db.Artists.AddRangeAsync(artist1, artist2, artist3);
        await _db.SaveChangesAsync();

        var result = await _repository.GetAllAsync(1, 2, CancellationToken.None);

        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(3);
        result.Page.Should().Be(1);
        result.PageSize.Should().Be(2);

        result.Items[0].Name.Should().Be("Beach House");
        result.Items[1].Name.Should().Be("Cocteau Twins");
    }

    [Test]
    public async Task ExistsByNameAsync_WhenNameExistsDifferentCase_ReturnsTrue()
    {
        var artist = new Artist("Magdalena Bay", "US");

        await _db.Artists.AddAsync(artist);
        await _db.SaveChangesAsync();

        var result = await _repository.ExistsByNameAsync(
        "magdalena bay",
        CancellationToken.None);

        result.Should().BeTrue();
    }

    [Test]
    public async Task ExistsByNameAsync_WhenNameDoesNotExist_ReturnsFalse()
    {
        var result = await _repository.ExistsByNameAsync(
        "Magdalena Bay",
        CancellationToken.None);

        result.Should().BeFalse();
    }

    [Test]
    public async Task ExistsByNameAsync_WithExcludeId_WhenOnlyMatchingArtistIsExcluded_ReturnsFalse()
    {
        var artist = new Artist("Magdalena Bay", "US");

        await _db.Artists.AddAsync(artist);
        await _db.SaveChangesAsync();

        var result = await _repository.ExistsByNameAsync(
        "Magdalena Bay",
        artist.Id,
        CancellationToken.None);

        result.Should().BeFalse();
    }

    [Test]
    public async Task ExistsByNameAsync_WithExcludeId_WhenAnotherArtistHasSameName_ReturnsTrue()
    {
        var artist1 = new Artist("Magdalena Bay", "US");
        var artist2 = new Artist("Cocteau Twins", "UK");

        await _db.Artists.AddRangeAsync(artist1, artist2);
        await _db.SaveChangesAsync();

        var result = await _repository.ExistsByNameAsync(
        "cocteau twins",
        artist1.Id,
        CancellationToken.None);

        result.Should().BeTrue();
    }

    [Test]
    public async Task UpdateAsync_SavesChanges()
    {
        var artist = new Artist("Magdalena Bay", "US");

        await _db.Artists.AddAsync(artist);
        await _db.SaveChangesAsync();

        artist.Rename("Mag Bay");
        artist.SetCountry("US");

        await _repository.UpdateAsync(artist, CancellationToken.None);

        _db.ChangeTracker.Clear();

        var savedArtist = await _db.Artists.FirstAsync(x => x.Id == artist.Id);

        savedArtist.Name.Should().Be("Mag Bay");
        savedArtist.Country.Should().Be("US");
    }

    [Test]
    public async Task DeleteAsync_RemovesArtist()
    {
        var artist = new Artist("Magdalena Bay", "US");

        await _db.Artists.AddAsync(artist);
        await _db.SaveChangesAsync();

        await _repository.DeleteAsync(artist, CancellationToken.None);

        var exists = await _db.Artists.AnyAsync(x => x.Id == artist.Id);

        exists.Should().BeFalse();
    }
}
