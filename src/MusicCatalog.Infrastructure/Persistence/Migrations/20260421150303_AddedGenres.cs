using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicCatalog.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedGenres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "genre",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_genre", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_genre_Title",
                table: "genre",
                column: "Title",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "genre");
        }
    }
}
