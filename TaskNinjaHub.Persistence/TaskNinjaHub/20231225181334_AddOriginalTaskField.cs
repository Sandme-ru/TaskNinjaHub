using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskNinjaHub.Persistence.TaskNinjaHub
{
    /// <inheritdoc />
    public partial class AddOriginalTaskField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_catalog_tasks_catalog_tasks_catalog_task_id",
                table: "catalog_tasks");

            migrationBuilder.RenameColumn(
                name: "catalog_task_id",
                table: "catalog_tasks",
                newName: "original_task_id");

            migrationBuilder.RenameIndex(
                name: "ix_catalog_tasks_catalog_task_id",
                table: "catalog_tasks",
                newName: "ix_catalog_tasks_original_task_id");

            migrationBuilder.AddForeignKey(
                name: "fk_catalog_tasks_catalog_tasks_original_task_id",
                table: "catalog_tasks",
                column: "original_task_id",
                principalTable: "catalog_tasks",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_catalog_tasks_catalog_tasks_original_task_id",
                table: "catalog_tasks");

            migrationBuilder.RenameColumn(
                name: "original_task_id",
                table: "catalog_tasks",
                newName: "catalog_task_id");

            migrationBuilder.RenameIndex(
                name: "ix_catalog_tasks_original_task_id",
                table: "catalog_tasks",
                newName: "ix_catalog_tasks_catalog_task_id");

            migrationBuilder.AddForeignKey(
                name: "fk_catalog_tasks_catalog_tasks_catalog_task_id",
                table: "catalog_tasks",
                column: "catalog_task_id",
                principalTable: "catalog_tasks",
                principalColumn: "id");
        }
    }
}
