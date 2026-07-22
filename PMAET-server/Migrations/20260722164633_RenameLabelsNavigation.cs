using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMAET_server.Migrations
{
    /// <inheritdoc />
    public partial class RenameLabelsNavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItemLabel_Label_InventoryItemLabelId",
                table: "InventoryItemLabel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InventoryItemLabel",
                table: "InventoryItemLabel");

            migrationBuilder.DropIndex(
                name: "IX_InventoryItemLabel_InventoryItemsId",
                table: "InventoryItemLabel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Label",
                table: "Label");

            migrationBuilder.RenameTable(
                name: "Label",
                newName: "Labels");

            migrationBuilder.RenameColumn(
                name: "InventoryItemLabelId",
                table: "InventoryItemLabel",
                newName: "LabelsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InventoryItemLabel",
                table: "InventoryItemLabel",
                columns: new[] { "InventoryItemsId", "LabelsId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Labels",
                table: "Labels",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItemLabel_LabelsId",
                table: "InventoryItemLabel",
                column: "LabelsId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItemLabel_Labels_LabelsId",
                table: "InventoryItemLabel",
                column: "LabelsId",
                principalTable: "Labels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItemLabel_Labels_LabelsId",
                table: "InventoryItemLabel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InventoryItemLabel",
                table: "InventoryItemLabel");

            migrationBuilder.DropIndex(
                name: "IX_InventoryItemLabel_LabelsId",
                table: "InventoryItemLabel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Labels",
                table: "Labels");

            migrationBuilder.RenameTable(
                name: "Labels",
                newName: "Label");

            migrationBuilder.RenameColumn(
                name: "LabelsId",
                table: "InventoryItemLabel",
                newName: "InventoryItemLabelId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InventoryItemLabel",
                table: "InventoryItemLabel",
                columns: new[] { "InventoryItemLabelId", "InventoryItemsId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Label",
                table: "Label",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItemLabel_InventoryItemsId",
                table: "InventoryItemLabel",
                column: "InventoryItemsId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItemLabel_Label_InventoryItemLabelId",
                table: "InventoryItemLabel",
                column: "InventoryItemLabelId",
                principalTable: "Label",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
