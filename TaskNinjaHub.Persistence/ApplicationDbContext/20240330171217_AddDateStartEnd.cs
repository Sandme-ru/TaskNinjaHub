using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskNinjaHub.Persistence.ApplicationDbContext
{
    /// <inheritdoc />
    public partial class AddDateStartEnd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "date_end",
                table: "catalog_tasks",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "date_start",
                table: "catalog_tasks",
                type: "timestamp without time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "date_end",
                table: "catalog_tasks");

            migrationBuilder.DropColumn(
                name: "date_start",
                table: "catalog_tasks");
        }
    }
}
