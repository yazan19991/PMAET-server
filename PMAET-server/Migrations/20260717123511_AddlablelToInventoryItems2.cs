using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMAET_server.Migrations
{
    /// <inheritdoc />
    public partial class AddlablelToInventoryItems2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Label");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Label");

            migrationBuilder.DropColumn(
                name: "url",
                table: "Label");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Label",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Label",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "url",
                table: "Label",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
