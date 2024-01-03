using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaskNinjaHub.Persistence.ApplicationDbContext
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "authors",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    role_name = table.Column<string>(type: "text", nullable: true),
                    user_created = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    user_updated = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    date_updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_authors", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "information_systems",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    user_created = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    user_updated = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    date_updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_information_systems", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "priorities",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_priorities", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "task_statuses",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    user_created = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    user_updated = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    date_updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_task_statuses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "catalog_tasks",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    task_author_id = table.Column<int>(type: "integer", nullable: true),
                    task_executor_id = table.Column<int>(type: "integer", nullable: true),
                    information_system_id = table.Column<int>(type: "integer", nullable: true),
                    priority_id = table.Column<int>(type: "integer", nullable: true),
                    task_status_id = table.Column<int>(type: "integer", nullable: true),
                    original_task_id = table.Column<int>(type: "integer", nullable: true),
                    user_created = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    user_updated = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    date_updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_catalog_tasks", x => x.id);
                    table.ForeignKey(
                        name: "fk_catalog_tasks_authors_task_author_id",
                        column: x => x.task_author_id,
                        principalTable: "authors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_catalog_tasks_authors_task_executor_id",
                        column: x => x.task_executor_id,
                        principalTable: "authors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_catalog_tasks_catalog_tasks_original_task_id",
                        column: x => x.original_task_id,
                        principalTable: "catalog_tasks",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_catalog_tasks_information_systems_information_system_id",
                        column: x => x.information_system_id,
                        principalTable: "information_systems",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_catalog_tasks_priorities_priority_id",
                        column: x => x.priority_id,
                        principalTable: "priorities",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_catalog_tasks_task_statuses_task_status_id",
                        column: x => x.task_status_id,
                        principalTable: "task_statuses",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "files",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    path = table.Column<string>(type: "text", nullable: true),
                    date_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    task_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_files", x => x.id);
                    table.ForeignKey(
                        name: "fk_files_catalog_tasks_task_id",
                        column: x => x.task_id,
                        principalTable: "catalog_tasks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "information_systems",
                columns: new[] { "id", "date_created", "date_updated", "name", "user_created", "user_updated" },
                values: new object[] { 1, null, null, "The main information system", null, null });

            migrationBuilder.InsertData(
                table: "priorities",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { 1, "The highest" },
                    { 2, "High" },
                    { 3, "Medium" },
                    { 4, "Low" }
                });

            migrationBuilder.InsertData(
                table: "task_statuses",
                columns: new[] { "id", "date_created", "date_updated", "name", "user_created", "user_updated" },
                values: new object[,]
                {
                    { 1, null, null, "Awaiting execution", null, null },
                    { 2, null, null, "At work", null, null },
                    { 3, null, null, "Awaiting verification", null, null },
                    { 4, null, null, "Done", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "ix_catalog_tasks_id",
                table: "catalog_tasks",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_catalog_tasks_information_system_id",
                table: "catalog_tasks",
                column: "information_system_id");

            migrationBuilder.CreateIndex(
                name: "ix_catalog_tasks_original_task_id",
                table: "catalog_tasks",
                column: "original_task_id");

            migrationBuilder.CreateIndex(
                name: "ix_catalog_tasks_priority_id",
                table: "catalog_tasks",
                column: "priority_id");

            migrationBuilder.CreateIndex(
                name: "ix_catalog_tasks_task_author_id",
                table: "catalog_tasks",
                column: "task_author_id");

            migrationBuilder.CreateIndex(
                name: "ix_catalog_tasks_task_executor_id",
                table: "catalog_tasks",
                column: "task_executor_id");

            migrationBuilder.CreateIndex(
                name: "ix_catalog_tasks_task_status_id",
                table: "catalog_tasks",
                column: "task_status_id");

            migrationBuilder.CreateIndex(
                name: "ix_files_task_id",
                table: "files",
                column: "task_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "files");

            migrationBuilder.DropTable(
                name: "catalog_tasks");

            migrationBuilder.DropTable(
                name: "authors");

            migrationBuilder.DropTable(
                name: "information_systems");

            migrationBuilder.DropTable(
                name: "priorities");

            migrationBuilder.DropTable(
                name: "task_statuses");
        }
    }
}
