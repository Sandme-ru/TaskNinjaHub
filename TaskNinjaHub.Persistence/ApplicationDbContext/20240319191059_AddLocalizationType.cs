using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskNinjaHub.Persistence.ApplicationDbContext
{
    /// <inheritdoc />
    public partial class AddLocalizationType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "localization_type",
                table: "authors",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "localization_type",
                table: "authors");
        }
    }
}
