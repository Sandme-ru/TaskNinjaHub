using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskNinjaHub.Persistence.ApplicationDbContext
{
    /// <inheritdoc />
    public partial class FixCatalogTaskNullProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_catalog_tasks_information_systems_information_system_id",
                table: "catalog_tasks");

            migrationBuilder.DropForeignKey(
                name: "fk_catalog_tasks_priorities_priority_id",
                table: "catalog_tasks");

            migrationBuilder.DropForeignKey(
                name: "fk_catalog_tasks_task_statuses_task_status_id",
                table: "catalog_tasks");

            migrationBuilder.DropForeignKey(
                name: "fk_catalog_tasks_task_types_task_type_id",
                table: "catalog_tasks");

            migrationBuilder.AlterColumn<int>(
                name: "task_type_id",
                table: "catalog_tasks",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "task_status_id",
                table: "catalog_tasks",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "task_executor_id",
                table: "catalog_tasks",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "task_author_id",
                table: "catalog_tasks",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "priority_id",
                table: "catalog_tasks",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "catalog_tasks",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "information_system_id",
                table: "catalog_tasks",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "task_types",
                keyColumn: "id",
                keyValue: 1,
                column: "date_created",
                value: new DateTime(2024, 4, 19, 23, 13, 58, 612, DateTimeKind.Local).AddTicks(5318));

            migrationBuilder.UpdateData(
                table: "task_types",
                keyColumn: "id",
                keyValue: 2,
                column: "date_created",
                value: new DateTime(2024, 4, 19, 23, 13, 58, 612, DateTimeKind.Local).AddTicks(5347));

            migrationBuilder.UpdateData(
                table: "task_types",
                keyColumn: "id",
                keyValue: 3,
                column: "date_created",
                value: new DateTime(2024, 4, 19, 23, 13, 58, 612, DateTimeKind.Local).AddTicks(5361));

            migrationBuilder.UpdateData(
                table: "task_types",
                keyColumn: "id",
                keyValue: 4,
                column: "date_created",
                value: new DateTime(2024, 4, 19, 23, 13, 58, 612, DateTimeKind.Local).AddTicks(5373));

            migrationBuilder.UpdateData(
                table: "task_types",
                keyColumn: "id",
                keyValue: 5,
                column: "date_created",
                value: new DateTime(2024, 4, 19, 23, 13, 58, 612, DateTimeKind.Local).AddTicks(5386));

            migrationBuilder.UpdateData(
                table: "task_types",
                keyColumn: "id",
                keyValue: 6,
                column: "date_created",
                value: new DateTime(2024, 4, 19, 23, 13, 58, 612, DateTimeKind.Local).AddTicks(5400));

            migrationBuilder.AddForeignKey(
                name: "fk_catalog_tasks_information_systems_information_system_id",
                table: "catalog_tasks",
                column: "information_system_id",
                principalTable: "information_systems",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_catalog_tasks_priorities_priority_id",
                table: "catalog_tasks",
                column: "priority_id",
                principalTable: "priorities",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_catalog_tasks_task_statuses_task_status_id",
                table: "catalog_tasks",
                column: "task_status_id",
                principalTable: "task_statuses",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_catalog_tasks_task_types_task_type_id",
                table: "catalog_tasks",
                column: "task_type_id",
                principalTable: "task_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_catalog_tasks_information_systems_information_system_id",
                table: "catalog_tasks");

            migrationBuilder.DropForeignKey(
                name: "fk_catalog_tasks_priorities_priority_id",
                table: "catalog_tasks");

            migrationBuilder.DropForeignKey(
                name: "fk_catalog_tasks_task_statuses_task_status_id",
                table: "catalog_tasks");

            migrationBuilder.DropForeignKey(
                name: "fk_catalog_tasks_task_types_task_type_id",
                table: "catalog_tasks");

            migrationBuilder.AlterColumn<int>(
                name: "task_type_id",
                table: "catalog_tasks",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "task_status_id",
                table: "catalog_tasks",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "task_executor_id",
                table: "catalog_tasks",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "task_author_id",
                table: "catalog_tasks",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "priority_id",
                table: "catalog_tasks",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "catalog_tasks",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<int>(
                name: "information_system_id",
                table: "catalog_tasks",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.UpdateData(
                table: "task_types",
                keyColumn: "id",
                keyValue: 1,
                column: "date_created",
                value: new DateTime(2024, 4, 1, 0, 23, 13, 594, DateTimeKind.Local).AddTicks(488));

            migrationBuilder.UpdateData(
                table: "task_types",
                keyColumn: "id",
                keyValue: 2,
                column: "date_created",
                value: new DateTime(2024, 4, 1, 0, 23, 13, 594, DateTimeKind.Local).AddTicks(559));

            migrationBuilder.UpdateData(
                table: "task_types",
                keyColumn: "id",
                keyValue: 3,
                column: "date_created",
                value: new DateTime(2024, 4, 1, 0, 23, 13, 594, DateTimeKind.Local).AddTicks(590));

            migrationBuilder.UpdateData(
                table: "task_types",
                keyColumn: "id",
                keyValue: 4,
                column: "date_created",
                value: new DateTime(2024, 4, 1, 0, 23, 13, 594, DateTimeKind.Local).AddTicks(606));

            migrationBuilder.UpdateData(
                table: "task_types",
                keyColumn: "id",
                keyValue: 5,
                column: "date_created",
                value: new DateTime(2024, 4, 1, 0, 23, 13, 594, DateTimeKind.Local).AddTicks(632));

            migrationBuilder.UpdateData(
                table: "task_types",
                keyColumn: "id",
                keyValue: 6,
                column: "date_created",
                value: new DateTime(2024, 4, 1, 0, 23, 13, 594, DateTimeKind.Local).AddTicks(646));

            migrationBuilder.AddForeignKey(
                name: "fk_catalog_tasks_information_systems_information_system_id",
                table: "catalog_tasks",
                column: "information_system_id",
                principalTable: "information_systems",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_catalog_tasks_priorities_priority_id",
                table: "catalog_tasks",
                column: "priority_id",
                principalTable: "priorities",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_catalog_tasks_task_statuses_task_status_id",
                table: "catalog_tasks",
                column: "task_status_id",
                principalTable: "task_statuses",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_catalog_tasks_task_types_task_type_id",
                table: "catalog_tasks",
                column: "task_type_id",
                principalTable: "task_types",
                principalColumn: "id");
        }
    }
}
