using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TaskNinjaHub.Persistence.ApplicationDbContext
{
    /// <inheritdoc />
    public partial class AddRelatedTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "related_tasks",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    main_task_id = table.Column<int>(type: "integer", nullable: false),
                    subordinate_task_id = table.Column<int>(type: "integer", nullable: false),
                    user_created = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    user_updated = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    date_created = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    date_updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_related_tasks", x => x.id);
                    table.ForeignKey(
                        name: "fk_related_tasks_catalog_tasks_main_task_id",
                        column: x => x.main_task_id,
                        principalTable: "catalog_tasks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_related_tasks_catalog_tasks_subordinate_task_id",
                        column: x => x.subordinate_task_id,
                        principalTable: "catalog_tasks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_related_tasks_main_task_id",
                table: "related_tasks",
                column: "main_task_id");

            migrationBuilder.CreateIndex(
                name: "ix_related_tasks_subordinate_task_id",
                table: "related_tasks",
                column: "subordinate_task_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "related_tasks");
        }
    }
}
