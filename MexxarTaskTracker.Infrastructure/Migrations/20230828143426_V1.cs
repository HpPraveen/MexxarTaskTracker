using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MexxarTaskTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class V1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoLists_User_UserId",
                table: "ToDoLists");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ToDoLists",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoLists_AspNetUsers_UserId",
                table: "ToDoLists",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ToDoLists_AspNetUsers_UserId",
                table: "ToDoLists");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "ToDoLists",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SysCreatedBy = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    SysCreatedOn = table.Column<DateTime>(type: "datetime2", maxLength: 128, nullable: true),
                    SysDeletedBy = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    SysDeletedOn = table.Column<DateTime>(type: "datetime2", maxLength: 128, nullable: true),
                    SysUpdatedBy = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    SysUpdatedOn = table.Column<DateTime>(type: "datetime2", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_ApplicationUserId",
                table: "User",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ToDoLists_User_UserId",
                table: "ToDoLists",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
