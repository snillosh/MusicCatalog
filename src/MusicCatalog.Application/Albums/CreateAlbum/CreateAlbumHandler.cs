using MediatR;
using MusicCatalog.Application.Artists;
using MusicCatalog.Application.Common.Results;
using MusicCatalog.Contracts.Albums;
using MusicCatalog.Domain.Albums;

namespace MusicCatalog.Application.Albums.CreateAlbum;

public sealed class CreateAlbumHandler(IArtistRepository artists, IAlbumRepository albums)
    : IRequestHandler<CreateAlbumCommand, Result<AlbumDto>>
{
    public async Task<Result<AlbumDto>> Handle(CreateAlbumCommand request, CancellationToken ct)
    {
        var artist = await artists.GetByIdAsync(request.ArtistId, ct);
        if (artist is null)
        {
            return Result<AlbumDto>.Fail("artists.notFound", "Artist not found.");
        }

        var title = request.Title.Trim();

        if (await albums.ExistsWithTitleAsync(request.ArtistId, title, ct))
        {
            return Result<AlbumDto>.Fail("albums.duplicate", "That artist already has an album with that title.");
        }

        var album = new Album(request.ArtistId, title, request.ReleaseYear);

        await albums.AddAsync(album, ct);

        return Result<AlbumDto>.Success(new AlbumDto(album.Id, album.ArtistId, album.Title, album.ReleaseYear));
    }
}
