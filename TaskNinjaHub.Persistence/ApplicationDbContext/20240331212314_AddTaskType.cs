using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaskNinjaHub.Persistence.ApplicationDbContext
{
    /// <inheritdoc />
    public partial class AddTaskType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "task_type_id",
                table: "catalog_tasks",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "task_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    user_created = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    user_updated = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    date_created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    date_updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_task_types", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "task_types",
                columns: new[] { "id", "date_created", "date_updated", "name", "user_created", "user_updated" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 4, 1, 0, 23, 13, 594, DateTimeKind.Local).AddTicks(488), null, "Bug", null, null },
                    { 2, new DateTime(2024, 4, 1, 0, 23, 13, 594, DateTimeKind.Local).AddTicks(559), null, "Feature", null, null },
                    { 3, new DateTime(2024, 4, 1, 0, 23, 13, 594, DateTimeKind.Local).AddTicks(590), null, "Epic", null, null },
                    { 4, new DateTime(2024, 4, 1, 0, 23, 13, 594, DateTimeKind.Local).AddTicks(606), null, "Testing", null, null },
                    { 5, new DateTime(2024, 4, 1, 0, 23, 13, 594, DateTimeKind.Local).AddTicks(632), null, "Task", null, null },
                    { 6, new DateTime(2024, 4, 1, 0, 23, 13, 594, DateTimeKind.Local).AddTicks(646), null, "Requirement", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "ix_catalog_tasks_task_type_id",
                table: "catalog_tasks",
                column: "task_type_id");

            migrationBuilder.AddForeignKey(
                name: "fk_catalog_tasks_task_types_task_type_id",
                table: "catalog_tasks",
                column: "task_type_id",
                principalTable: "task_types",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_catalog_tasks_task_types_task_type_id",
                table: "catalog_tasks");

            migrationBuilder.DropTable(
                name: "task_types");

            migrationBuilder.DropIndex(
                name: "ix_catalog_tasks_task_type_id",
                table: "catalog_tasks");

            migrationBuilder.DropColumn(
                name: "task_type_id",
                table: "catalog_tasks");
        }
    }
}
