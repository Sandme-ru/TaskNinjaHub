using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskNinjaHub.Persistence.ApplicationDbContext
{
    /// <inheritdoc />
    public partial class DeleteFilePath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "path",
                table: "files");

            migrationBuilder.AlterColumn<int>(
                name: "localization_type",
                table: "authors",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "path",
                table: "files",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "localization_type",
                table: "authors",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
