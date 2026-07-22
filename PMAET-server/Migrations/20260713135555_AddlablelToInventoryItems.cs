using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMAET_server.Migrations
{
    /// <inheritdoc />
    public partial class AddlablelToInventoryItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Label",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Label", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InventoryItemLabel",
                columns: table => new
                {
                    InventoryItemLabelId = table.Column<int>(type: "int", nullable: false),
                    InventoryItemsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItemLabel", x => new { x.InventoryItemLabelId, x.InventoryItemsId });
                    table.ForeignKey(
                        name: "FK_InventoryItemLabel_InventoryItems_InventoryItemsId",
                        column: x => x.InventoryItemsId,
                        principalTable: "InventoryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InventoryItemLabel_Label_InventoryItemLabelId",
                        column: x => x.InventoryItemLabelId,
                        principalTable: "Label",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItemLabel_InventoryItemsId",
                table: "InventoryItemLabel",
                column: "InventoryItemsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventoryItemLabel");

            migrationBuilder.DropTable(
                name: "Label");
        }
    }
}
