using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MusicCatalog.Domain.Albums;
using MusicCatalog.Domain.Artists;
using MusicCatalog.Infrastructure.Persistence;
using MusicCatalog.Infrastructure.Persistence.Repositories;
using NUnit.Framework;
using Testcontainers.PostgreSql;

namespace MusicCatalog.Infrastructure.Tests;

[TestFixture]
public class AlbumRepositoryTests
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

        _repository = new AlbumRepository(_db);
    }

    [TearDown]
    public async Task TearDown() => await _db.DisposeAsync();
    private PostgreSqlContainer _postgres = null!;
    private MusicCatalogDbContext _db = null!;
    private AlbumRepository _repository = null!;

    [Test]
    public async Task AddAsync_SavesAlbum()
    {
        var artist = new Artist("Magdalena Bay", "US");

        await _db.Artists.AddAsync(artist);
        await _db.SaveChangesAsync();

        var album = new Album(artist.Id, "Imaginal Disk", 2024);

        await _repository.AddAsync(album, CancellationToken.None);

        var savedAlbum = await _db.Albums.FirstOrDefaultAsync(x => x.Id == album.Id);

        savedAlbum.Should().NotBeNull();
        savedAlbum!.Title.Should().Be("Imaginal Disk");
        savedAlbum.ReleaseYear.Should().Be(2024);
    }

    [Test]
    public async Task GetByIdAsync_ReturnsAlbum()
    {
        var artist = new Artist("Magdalena Bay", "US");
        var album = new Album(artist.Id, "Imaginal Disk", 2024);

        await _db.Artists.AddAsync(artist);
        await _db.Albums.AddAsync(album);
        await _db.SaveChangesAsync();

        var result = await _repository.GetByIdAsync(album.Id, CancellationToken.None);

        result.Should().NotBeNull();
        result!.Id.Should().Be(album.Id);
        result.Title.Should().Be("Imaginal Disk");
    }

    [Test]
    public async Task GetByIdAsync_ReturnsUntrackedEntity()
    {
        var artist = new Artist("Magdalena Bay", "US");
        var album = new Album(artist.Id, "Imaginal Disk", 2024);

        await _db.Artists.AddAsync(artist);
        await _db.Albums.AddAsync(album);
        await _db.SaveChangesAsync();

        _db.ChangeTracker.Clear();

        var result = await _repository.GetByIdAsync(album.Id, CancellationToken.None);

        result.Should().NotBeNull();

        var tracked = _db.ChangeTracker
            .Entries<Album>()
            .Any(x => x.Entity.Id == album.Id && x.State != EntityState.Detached);

        tracked.Should().BeFalse();
    }

    [Test]
    public async Task GetByIdTrackedAsync_ReturnsTrackedEntity()
    {
        var artist = new Artist("Magdalena Bay", "US");
        var album = new Album(artist.Id, "Imaginal Disk", 2024);

        await _db.Artists.AddAsync(artist);
        await _db.Albums.AddAsync(album);
        await _db.SaveChangesAsync();

        var result = await _repository.GetByIdTrackedAsync(album.Id, CancellationToken.None);

        var tracked = _db.ChangeTracker
            .Entries<Album>()
            .Any(x => x.Entity.Id == album.Id && x.State != EntityState.Detached);

        tracked.Should().BeTrue();
    }

    [Test]
    public async Task ExistsWithTitleAsync_WhenMatchingArtistAndTitleExists_ReturnsTrue()
    {
        var artist = new Artist("Magdalena Bay", "US");
        var album = new Album(artist.Id, "Imaginal Disk", 2024);

        await _db.Artists.AddAsync(artist);
        await _db.Albums.AddAsync(album);
        await _db.SaveChangesAsync();

        var result = await _repository.ExistsWithTitleAsync(
        artist.Id,
        "  imaginal disk  ",
        CancellationToken.None);

        result.Should().BeTrue();
    }

    [Test]
    public async Task ExistsWithTitleAsync_WhenDifferentArtistHasSameTitle_ReturnsFalse()
    {
        var artist1 = new Artist("Magdalena Bay", "US");
        var artist2 = new Artist("Cocteau Twins", "UK");

        var album = new Album(artist1.Id, "Imaginal Disk", 2024);

        await _db.Artists.AddRangeAsync(artist1, artist2);
        await _db.Albums.AddAsync(album);
        await _db.SaveChangesAsync();

        var result = await _repository.ExistsWithTitleAsync(
        artist2.Id,
        "Imaginal Disk",
        CancellationToken.None);

        result.Should().BeFalse();
    }

    [Test]
    public async Task DeleteAsync_RemovesAlbum()
    {
        var artist = new Artist("Magdalena Bay", "US");
        var album = new Album(artist.Id, "Imaginal Disk", 2024);

        await _db.Artists.AddAsync(artist);
        await _db.Albums.AddAsync(album);
        await _db.SaveChangesAsync();

        await _repository.DeleteAsync(album, CancellationToken.None);

        var exists = await _db.Albums.AnyAsync(x => x.Id == album.Id);

        exists.Should().BeFalse();
    }

    [Test]
    public async Task GetByArtistIdAsync_ReturnsPagedAlbums()
    {
        var artist = new Artist("Magdalena Bay", "US");

        var album1 = new Album(artist.Id, "A Album", 2023);
        var album2 = new Album(artist.Id, "B Album", 2024);
        var album3 = new Album(artist.Id, "C Album", 2025);

        await _db.Artists.AddAsync(artist);
        await _db.Albums.AddRangeAsync(album1, album2, album3);
        await _db.SaveChangesAsync();

        var result = await _repository.GetByArtistIdAsync(
        artist.Id,
        1,
        2,
        CancellationToken.None);

        result.Items.Should().HaveCount(2);
        result.TotalCount.Should().Be(3);

        result.Items[0].Title.Should().Be("A Album");
        result.Items[1].Title.Should().Be("B Album");
    }

    [Test]
    public async Task GetAllWithArtistNameAsync_WhenReleasedAfterProvided_FiltersAlbums()
    {
        var artist = new Artist("Magdalena Bay", "US");

        var oldAlbum = new Album(artist.Id, "Mercurial World", 2021);
        var newAlbum = new Album(artist.Id, "Imaginal Disk", 2024);

        await _db.Artists.AddAsync(artist);
        await _db.Albums.AddRangeAsync(oldAlbum, newAlbum);
        await _db.SaveChangesAsync();

        var result = await _repository.GetAllWithArtistNameAsync(
        1,
        10,
        2023,
        CancellationToken.None);

        result.Items.Should().HaveCount(1);

        result.Items[0].Title.Should().Be("Imaginal Disk");
        result.Items[0].ArtistName.Should().Be("Magdalena Bay");
    }
}
