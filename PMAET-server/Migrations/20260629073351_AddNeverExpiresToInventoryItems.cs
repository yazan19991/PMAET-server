using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMAET_server.Migrations
{
    /// <inheritdoc />
    public partial class AddNeverExpiresToInventoryItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NeverExpires",
                table: "InventoryItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NeverExpires",
                table: "InventoryItems");
        }
    }
}
