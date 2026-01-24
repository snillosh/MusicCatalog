using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MusicCatalog.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddArtistCountry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Artist",
                type: "character varying(2)",
                unicode: false,
                maxLength: 2,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "Artist");
        }
    }
}
