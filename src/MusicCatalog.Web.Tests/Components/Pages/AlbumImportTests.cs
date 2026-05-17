using Bunit;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using MusicCatalog.ApiClient;
using MusicCatalog.Contracts.Albums;
using MusicCatalog.Contracts.Tracks;
using MusicCatalog.ExternalMetadata;
using MusicCatalog.Importing;
using MusicCatalog.Web.Components.Pages;
using NSubstitute;
using NUnit.Framework;

namespace MusicCatalog.Web.Tests.Components.Pages;

[TestFixture]
public class AlbumImportTests
{

    [SetUp]
    public void SetUp()
    {
        _context = new BunitContext();

        _musicService = Substitute.For<IMusicMetadataService>();
        _albumImportService = Substitute.For<IAlbumImportService>();
        _tokenProvider = Substitute.For<IAccessTokenStore>();

        _context.Services.AddSingleton(_musicService);
        _context.Services.AddSingleton(_albumImportService);
        _context.Services.AddSingleton(_tokenProvider);
    }

    [TearDown]
    public void TearDown() => _context.Dispose();
    private BunitContext _context = null!;
    private IMusicMetadataService _musicService = null!;
    private IAlbumImportService _albumImportService = null!;
    private IAccessTokenStore _tokenProvider = null!;

    [Test]
    public void AlbumImport_RendersSearchForm()
    {
        var component = _context.Render<AlbumImport>();

        component.Markup.Should().Contain("Add Album");
        component.Markup.Should().Contain("Album name");
        component.Markup.Should().Contain("Search for an album...");
        component.Markup.Should().Contain("Search");
    }

    [Test]
    public void SearchAlbums_WhenSearchClicked_ShowsResults()
    {
        var releaseId = Guid.NewGuid();

        _musicService.FindSimpleAlbumsAsync("Imaginal Disk")
            .Returns(
            new List<AlbumSearchResult>
            {
                new(
                Guid.NewGuid(),
                releaseId,
                "Imaginal Disk",
                "Magdalena Bay",
                "2024")
            });

        var component = _context.Render<AlbumImport>();

        component.Find("input").Change("Imaginal Disk");
        component.Find("button.btn-primary").Click();

        component.Markup.Should().Contain("Search Results");
        component.Markup.Should().Contain("Imaginal Disk");
        component.Markup.Should().Contain("Magdalena Bay");
        component.Markup.Should().Contain("2024 · 15 tracks");

        _musicService.Received(1)
            .FindSimpleAlbumsAsync("Imaginal Disk");
    }

    [Test]
    public void SelectAlbum_WhenResultClicked_ShowsAlbumPreview()
    {
        var releaseId = Guid.NewGuid();

        _musicService.FindSimpleAlbumsAsync("Imaginal Disk")
            .Returns(
            new List<AlbumSearchResult>
            {
                new(
                Guid.NewGuid(),
                releaseId,
                "Imaginal Disk",
                "Magdalena Bay",
                "2024")
            });

        _musicService.LookupAlbumImportPreviewAsync(releaseId)
            .Returns(
            new AlbumImportPreview(
            releaseId,
            "Imaginal Disk",
            "Magdalena Bay",
            "2024",
            new List<TrackPreview>
            {
                new("She Looked Like Me!", 1, 187), new("Killing Time", 2, 225)
            }));

        var component = _context.Render<AlbumImport>();

        component.Find("input").Change("Imaginal Disk");
        component.Find("button.btn-primary").Click();
        component.Find(".list-group-item").Click();

        component.Markup.Should().Contain("Artist:");
        component.Markup.Should().Contain("Magdalena Bay");
        component.Markup.Should().Contain("Release date:");
        component.Markup.Should().Contain("2024");
        component.Markup.Should().Contain("She Looked Like Me!");
        component.Markup.Should().Contain("Killing Time");
        component.Markup.Should().Contain("Add to Catalogue");

        _musicService.Received(1)
            .LookupAlbumImportPreviewAsync(releaseId);
    }

    [Test]
    public void SelectAlbum_WhenResultClicked_SetsCoverImageUrl()
    {
        var releaseId = Guid.NewGuid();

        _musicService.FindSimpleAlbumsAsync("Imaginal Disk")
            .Returns(
            new List<AlbumSearchResult>
            {
                new(
                Guid.NewGuid(),
                releaseId,
                "Imaginal Disk",
                "Magdalena Bay",
                "2024")
            });

        _musicService.LookupAlbumImportPreviewAsync(releaseId)
            .Returns(
            new AlbumImportPreview(
            releaseId,
            "Imaginal Disk",
            "Magdalena Bay",
            "2024",
            new List<TrackPreview>()));

        var component = _context.Render<AlbumImport>();

        component.Find("input").Change("Imaginal Disk");
        component.Find("button.btn-primary").Click();
        component.Find(".list-group-item").Click();

        var image = component.Find("img");

        image.GetAttribute("src")
            .Should()
            .Be($"https://coverartarchive.org/release/{releaseId}/front-250");
    }

    [Test]
    public void AddAlbum_WhenPreviewLoaded_CallsImportService()
    {
        _tokenProvider.GetAccessTokenAsync().Returns("token");

        var releaseId = Guid.NewGuid();

        var preview = new AlbumImportPreview(
        releaseId,
        "Imaginal Disk",
        "Magdalena Bay",
        "2024",
        new List<TrackPreview>
        {
            new("She Looked Like Me!", 1, 187)
        });

        _musicService.FindSimpleAlbumsAsync("Imaginal Disk")
            .Returns(
            new List<AlbumSearchResult>
            {
                new(
                Guid.NewGuid(),
                releaseId,
                "Imaginal Disk",
                "Magdalena Bay",
                "2024")
            });

        _musicService.LookupAlbumImportPreviewAsync(releaseId)
            .Returns(preview);

        var component = _context.Render<AlbumImport>();

        component.Find("input").Change("Imaginal Disk");
        component.Find("button.btn-primary").Click();
        component.Find(".list-group-item").Click();
        component.Find("button.btn-success").Click();

        _albumImportService.Received(1)
            .ImportAlbumAsync(preview, Arg.Any<CancellationToken>());

        component.Markup.Should().Contain("Added 'Imaginal Disk' to your catalogue.");
    }
}
