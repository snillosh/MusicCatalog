using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicCatalog.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Album_Artist_ArtistId",
                table: "Album");

            migrationBuilder.DropForeignKey(
                name: "FK_Track_Album_AlbumId",
                table: "Track");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Track",
                table: "Track");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Artist",
                table: "Artist");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Album",
                table: "Album");

            migrationBuilder.RenameTable(
                name: "Track",
                newName: "track");

            migrationBuilder.RenameTable(
                name: "Artist",
                newName: "artist");

            migrationBuilder.RenameTable(
                name: "Album",
                newName: "album");

            migrationBuilder.RenameIndex(
                name: "IX_Track_AlbumId_TrackNumber",
                table: "track",
                newName: "IX_track_AlbumId_TrackNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Artist_Name",
                table: "artist",
                newName: "IX_artist_Name");

            migrationBuilder.RenameIndex(
                name: "IX_Album_ArtistId_Title",
                table: "album",
                newName: "IX_album_ArtistId_Title");

            migrationBuilder.AddPrimaryKey(
                name: "PK_track",
                table: "track",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_artist",
                table: "artist",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_album",
                table: "album",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_album_artist_ArtistId",
                table: "album",
                column: "ArtistId",
                principalTable: "artist",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_track_album_AlbumId",
                table: "track",
                column: "AlbumId",
                principalTable: "album",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_album_artist_ArtistId",
                table: "album");

            migrationBuilder.DropForeignKey(
                name: "FK_track_album_AlbumId",
                table: "track");

            migrationBuilder.DropPrimaryKey(
                name: "PK_track",
                table: "track");

            migrationBuilder.DropPrimaryKey(
                name: "PK_artist",
                table: "artist");

            migrationBuilder.DropPrimaryKey(
                name: "PK_album",
                table: "album");

            migrationBuilder.RenameTable(
                name: "track",
                newName: "Track");

            migrationBuilder.RenameTable(
                name: "artist",
                newName: "Artist");

            migrationBuilder.RenameTable(
                name: "album",
                newName: "Album");

            migrationBuilder.RenameIndex(
                name: "IX_track_AlbumId_TrackNumber",
                table: "Track",
                newName: "IX_Track_AlbumId_TrackNumber");

            migrationBuilder.RenameIndex(
                name: "IX_artist_Name",
                table: "Artist",
                newName: "IX_Artist_Name");

            migrationBuilder.RenameIndex(
                name: "IX_album_ArtistId_Title",
                table: "Album",
                newName: "IX_Album_ArtistId_Title");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Track",
                table: "Track",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Artist",
                table: "Artist",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Album",
                table: "Album",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Album_Artist_ArtistId",
                table: "Album",
                column: "ArtistId",
                principalTable: "Artist",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Track_Album_AlbumId",
                table: "Track",
                column: "AlbumId",
                principalTable: "Album",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
