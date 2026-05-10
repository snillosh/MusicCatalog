using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MusicCatalog.Domain.Albums;
using MusicCatalog.Domain.Artists;
using MusicCatalog.Domain.Tracks;
using MusicCatalog.Infrastructure.Persistence;
using MusicCatalog.Infrastructure.Persistence.Repositories;
using NUnit.Framework;
using Testcontainers.PostgreSql;

namespace MusicCatalog.Infrastructure.Tests;

[TestFixture]
public class TrackRepositoryTests
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

        _repository = new TrackRepository(_db);
    }

    [TearDown]
    public async Task TearDown() => await _db.DisposeAsync();
    private PostgreSqlContainer _postgres = null!;
    private MusicCatalogDbContext _db = null!;
    private TrackRepository _repository = null!;

    [Test]
    public async Task AddAsync_SavesTrack()
    {
        var artist = new Artist("Magdalena Bay", "US");
        var album = new Album(artist.Id, "Imaginal Disk", 2024);
        var track = new Track(album.Id, 1, "She Looked Like Me!", 187);

        await _db.Artists.AddAsync(artist);
        await _db.Albums.AddAsync(album);
        await _db.SaveChangesAsync();

        await _repository.AddAsync(track, CancellationToken.None);

        var savedTrack = await _db.Tracks.FirstOrDefaultAsync(x => x.Id == track.Id);

        savedTrack.Should().NotBeNull();
        savedTrack!.AlbumId.Should().Be(album.Id);
        savedTrack.TrackNumber.Should().Be(1);
        savedTrack.Title.Should().Be("She Looked Like Me!");
        savedTrack.DurationSeconds.Should().Be(187);
    }

    [Test]
    public async Task GetByAlbumIdAsync_ReturnsTracksForAlbumOrderedByTrackNumber()
    {
        var artist = new Artist("Magdalena Bay", "US");
        var album = new Album(artist.Id, "Imaginal Disk", 2024);
        var otherAlbum = new Album(artist.Id, "Mercurial World", 2021);

        var track2 = new Track(album.Id, 2, "Killing Time", 225);
        var track1 = new Track(album.Id, 1, "She Looked Like Me!", 187);
        var otherAlbumTrack = new Track(otherAlbum.Id, 1, "The End", 199);

        await _db.Artists.AddAsync(artist);
        await _db.Albums.AddRangeAsync(album, otherAlbum);
        await _db.Tracks.AddRangeAsync(track2, track1, otherAlbumTrack);
        await _db.SaveChangesAsync();

        var result = await _repository.GetByAlbumIdAsync(album.Id, CancellationToken.None);

        result.Should().HaveCount(2);

        result[0].Id.Should().Be(track1.Id);
        result[0].TrackNumber.Should().Be(1);
        result[0].Title.Should().Be("She Looked Like Me!");

        result[1].Id.Should().Be(track2.Id);
        result[1].TrackNumber.Should().Be(2);
        result[1].Title.Should().Be("Killing Time");
    }

    [Test]
    public async Task GetByAlbumIdAsync_WhenAlbumHasNoTracks_ReturnsEmptyList()
    {
        var albumId = Guid.NewGuid();

        var result = await _repository.GetByAlbumIdAsync(albumId, CancellationToken.None);

        result.Should().BeEmpty();
    }

    [Test]
    public async Task GetByAlbumIdAsync_ReturnsUntrackedEntities()
    {
        var artist = new Artist("Magdalena Bay", "US");
        var album = new Album(artist.Id, "Imaginal Disk", 2024);
        var track = new Track(album.Id, 1, "She Looked Like Me!", 187);

        await _db.Artists.AddAsync(artist);
        await _db.Albums.AddAsync(album);
        await _db.Tracks.AddAsync(track);
        await _db.SaveChangesAsync();

        _db.ChangeTracker.Clear();

        var result = await _repository.GetByAlbumIdAsync(album.Id, CancellationToken.None);

        result.Should().HaveCount(1);

        var tracked = _db.ChangeTracker
            .Entries<Track>()
            .Any(x => x.Entity.Id == track.Id && x.State != EntityState.Detached);

        tracked.Should().BeFalse();
    }

    [Test]
    public async Task ExistsTrackNumberAsync_WhenTrackNumberExistsForAlbum_ReturnsTrue()
    {
        var artist = new Artist("Magdalena Bay", "US");
        var album = new Album(artist.Id, "Imaginal Disk", 2024);
        var track = new Track(album.Id, 1, "She Looked Like Me!", 187);

        await _db.Artists.AddAsync(artist);
        await _db.Albums.AddAsync(album);
        await _db.Tracks.AddAsync(track);
        await _db.SaveChangesAsync();

        var result = await _repository.ExistsTrackNumberAsync(
        album.Id,
        1,
        CancellationToken.None);

        result.Should().BeTrue();
    }

    [Test]
    public async Task ExistsTrackNumberAsync_WhenTrackNumberExistsForDifferentAlbum_ReturnsFalse()
    {
        var artist = new Artist("Magdalena Bay", "US");
        var album = new Album(artist.Id, "Imaginal Disk", 2024);
        var otherAlbum = new Album(artist.Id, "Mercurial World", 2021);
        var track = new Track(otherAlbum.Id, 1, "The End", 199);

        await _db.Artists.AddAsync(artist);
        await _db.Albums.AddRangeAsync(album, otherAlbum);
        await _db.Tracks.AddAsync(track);
        await _db.SaveChangesAsync();

        var result = await _repository.ExistsTrackNumberAsync(
        album.Id,
        1,
        CancellationToken.None);

        result.Should().BeFalse();
    }

    [Test]
    public async Task ExistsTrackNumberAsync_WhenTrackNumberDoesNotExist_ReturnsFalse()
    {
        var result = await _repository.ExistsTrackNumberAsync(
        Guid.NewGuid(),
        1,
        CancellationToken.None);

        result.Should().BeFalse();
    }
}
