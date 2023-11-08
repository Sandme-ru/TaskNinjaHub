using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaskNinjaHub.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InformationSystems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserCreated = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UserUpdated = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InformationSystems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Priorities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Priorities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserCreated = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UserUpdated = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    UserCreated = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UserUpdated = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Authors_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CatalogTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TaskAuthorId = table.Column<int>(type: "int", nullable: true),
                    TaskExecutorId = table.Column<int>(type: "int", nullable: true),
                    InformationSystemId = table.Column<int>(type: "int", nullable: true),
                    PriorityId = table.Column<int>(type: "int", nullable: true),
                    TaskStatusId = table.Column<int>(type: "int", nullable: true),
                    CatalogTaskID = table.Column<int>(type: "int", nullable: true),
                    UserCreated = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    UserUpdated = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatalogTasks_Authors_TaskAuthorId",
                        column: x => x.TaskAuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CatalogTasks_Authors_TaskExecutorId",
                        column: x => x.TaskExecutorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CatalogTasks_CatalogTasks_CatalogTaskID",
                        column: x => x.CatalogTaskID,
                        principalTable: "CatalogTasks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CatalogTasks_InformationSystems_InformationSystemId",
                        column: x => x.InformationSystemId,
                        principalTable: "InformationSystems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CatalogTasks_Priorities_PriorityId",
                        column: x => x.PriorityId,
                        principalTable: "Priorities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CatalogTasks_TaskStatuses_TaskStatusId",
                        column: x => x.TaskStatusId,
                        principalTable: "TaskStatuses",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AvatarPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TaskId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Files_CatalogTasks_TaskId",
                        column: x => x.TaskId,
                        principalTable: "CatalogTasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "InformationSystems",
                columns: new[] { "Id", "DateCreated", "DateUpdated", "Name", "UserCreated", "UserUpdated" },
                values: new object[] { 1, null, null, "The main information system", null, null });

            migrationBuilder.InsertData(
                table: "Priorities",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "The highest" },
                    { 2, "High" },
                    { 3, "Medium" },
                    { 4, "Low" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Developer" },
                    { 2, "Analyst" },
                    { 3, "Support" },
                    { 4, "Tester" }
                });

            migrationBuilder.InsertData(
                table: "TaskStatuses",
                columns: new[] { "Id", "DateCreated", "DateUpdated", "Name", "UserCreated", "UserUpdated" },
                values: new object[,]
                {
                    { 1, null, null, "Awaiting execution", null, null },
                    { 2, null, null, "At work", null, null },
                    { 3, null, null, "Awaiting verification", null, null },
                    { 4, null, null, "Done", null, null }
                });

            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "DateCreated", "DateUpdated", "Name", "RoleId", "UserCreated", "UserUpdated" },
                values: new object[,]
                {
                    { 1, null, null, "First developer", 1, null, null },
                    { 2, null, null, "Second developer", 1, null, null },
                    { 3, null, null, "First analyst", 2, null, null },
                    { 4, null, null, "Second analyst", 2, null, null },
                    { 5, null, null, "First support", 3, null, null },
                    { 6, null, null, "Second support", 3, null, null },
                    { 7, null, null, "First tester", 4, null, null },
                    { 8, null, null, "Second tester", 4, null, null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AuthorId", "AvatarPath", "Password", "Username" },
                values: new object[,]
                {
                    { 1, 1, null, "user1", "user1" },
                    { 2, 2, null, "user2", "user2" },
                    { 3, 3, null, "user3", "user3" },
                    { 4, 4, null, "user4", "user4" },
                    { 5, 5, null, "user5", "user5" },
                    { 6, 6, null, "user6", "user6" },
                    { 7, 7, null, "user7", "user7" },
                    { 8, 8, null, "user8", "user8" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Authors_RoleId",
                table: "Authors",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogTasks_CatalogTaskID",
                table: "CatalogTasks",
                column: "CatalogTaskID");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogTasks_Id",
                table: "CatalogTasks",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CatalogTasks_InformationSystemId",
                table: "CatalogTasks",
                column: "InformationSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogTasks_PriorityId",
                table: "CatalogTasks",
                column: "PriorityId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogTasks_TaskAuthorId",
                table: "CatalogTasks",
                column: "TaskAuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogTasks_TaskExecutorId",
                table: "CatalogTasks",
                column: "TaskExecutorId");

            migrationBuilder.CreateIndex(
                name: "IX_CatalogTasks_TaskStatusId",
                table: "CatalogTasks",
                column: "TaskStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Files_TaskId",
                table: "Files",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_AuthorId",
                table: "Users",
                column: "AuthorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "CatalogTasks");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "InformationSystems");

            migrationBuilder.DropTable(
                name: "Priorities");

            migrationBuilder.DropTable(
                name: "TaskStatuses");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
