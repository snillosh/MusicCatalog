using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicCatalog.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedMusicBrainzIdsToAlbums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MusicBrainzReleaseGroupId",
                table: "album",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "MusicBrainzReleaseId",
                table: "album",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MusicBrainzReleaseGroupId",
                table: "album");

            migrationBuilder.DropColumn(
                name: "MusicBrainzReleaseId",
                table: "album");
        }
    }
}
