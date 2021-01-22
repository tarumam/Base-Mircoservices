using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseProject.Catalog.Infra.Migrations
{
    public partial class syncWithWeb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SyncWithWeb",
                table: "Product",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SyncWithWeb",
                table: "Product");
        }
    }
}
