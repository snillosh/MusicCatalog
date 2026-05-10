using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MusicCatalog.Domain.Genre;
using MusicCatalog.Infrastructure.Persistence;
using MusicCatalog.Infrastructure.Persistence.Repositories;
using NUnit.Framework;
using Testcontainers.PostgreSql;

namespace MusicCatalog.Infrastructure.Tests;

[TestFixture]
public class GenreRepositoryTests
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

        _repository = new GenreRepository(_db);
    }

    [TearDown]
    public async Task TearDown() => await _db.DisposeAsync();
    private PostgreSqlContainer _postgres = null!;
    private MusicCatalogDbContext _db = null!;
    private GenreRepository _repository = null!;

    [Test]
    public async Task AddAsync_SavesGenre()
    {
        var genre = new Genre("Dream Pop");

        await _repository.AddAsync(genre, CancellationToken.None);

        var savedGenre = await _db.Genres.FirstOrDefaultAsync(x => x.Id == genre.Id);

        savedGenre.Should().NotBeNull();
        savedGenre!.Title.Should().Be("Dream Pop");
    }

    [Test]
    public async Task GetByIdAsync_WhenGenreExists_ReturnsGenre()
    {
        var genre = new Genre("Dream Pop");

        await _db.Genres.AddAsync(genre);
        await _db.SaveChangesAsync();

        var result = await _repository.GetByIdAsync(genre.Id, CancellationToken.None);

        result.Should().NotBeNull();
        result!.Id.Should().Be(genre.Id);
        result.Title.Should().Be("Dream Pop");
    }

    [Test]
    public async Task GetByIdAsync_WhenGenreDoesNotExist_ReturnsNull()
    {
        var result = await _repository.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

        result.Should().BeNull();
    }

    [Test]
    public async Task GetByIdAsync_ReturnsUntrackedEntity()
    {
        var genre = new Genre("Dream Pop");

        await _db.Genres.AddAsync(genre);
        await _db.SaveChangesAsync();

        _db.ChangeTracker.Clear();

        var result = await _repository.GetByIdAsync(genre.Id, CancellationToken.None);

        result.Should().NotBeNull();

        var tracked = _db.ChangeTracker
            .Entries<Genre>()
            .Any(x => x.Entity.Id == genre.Id && x.State != EntityState.Detached);

        tracked.Should().BeFalse();
    }

    [Test]
    public async Task GetByIdTrackedAsync_ReturnsTrackedEntity()
    {
        var genre = new Genre("Dream Pop");

        await _db.Genres.AddAsync(genre);
        await _db.SaveChangesAsync();

        _db.ChangeTracker.Clear();

        var result = await _repository.GetByIdTrackedAsync(genre.Id, CancellationToken.None);

        result.Should().NotBeNull();

        var tracked = _db.ChangeTracker
            .Entries<Genre>()
            .Any(x => x.Entity.Id == genre.Id && x.State != EntityState.Detached);

        tracked.Should().BeTrue();
    }

    [Test]
    public async Task ExistsWithTitleAsync_WhenTitleExistsDifferentCase_ReturnsTrue()
    {
        var genre = new Genre("Dream Pop");

        await _db.Genres.AddAsync(genre);
        await _db.SaveChangesAsync();

        var result = await _repository.ExistsWithTitleAsync(
        "dream pop",
        null,
        CancellationToken.None);

        result.Should().BeTrue();
    }

    [Test]
    public async Task ExistsWithTitleAsync_WhenTitleDoesNotExist_ReturnsFalse()
    {
        var result = await _repository.ExistsWithTitleAsync(
        "Dream Pop",
        null,
        CancellationToken.None);

        result.Should().BeFalse();
    }

    [Test]
    public async Task ExistsWithTitleAsync_WithExcludeId_WhenOnlyMatchingGenreIsExcluded_ReturnsFalse()
    {
        var genre = new Genre("Dream Pop");

        await _db.Genres.AddAsync(genre);
        await _db.SaveChangesAsync();

        var result = await _repository.ExistsWithTitleAsync(
        "dream pop",
        genre.Id,
        CancellationToken.None);

        result.Should().BeFalse();
    }

    [Test]
    public async Task ExistsWithTitleAsync_WithExcludeId_WhenDifferentGenreHasTitle_ReturnsTrue()
    {
        var genre1 = new Genre("Dream Pop");
        var genre2 = new Genre("Shoegaze");

        await _db.Genres.AddRangeAsync(genre1, genre2);
        await _db.SaveChangesAsync();

        var result = await _repository.ExistsWithTitleAsync(
        "shoegaze",
        genre1.Id,
        CancellationToken.None);

        result.Should().BeTrue();
    }
}
