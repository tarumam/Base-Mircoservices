using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseProject.ShoppingCart.API.Migrations
{
    public partial class initialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ShoppingCartClients",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ClientId = table.Column<Guid>(nullable: false),
                    TotalValue = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCartClients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCartItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ProductId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", nullable: true),
                    Amount = table.Column<int>(nullable: false),
                    Value = table.Column<decimal>(nullable: false),
                    Image = table.Column<string>(type: "varchar(100)", nullable: true),
                    ShoppingCartId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingCartItems_ShoppingCartClients_ShoppingCartId",
                        column: x => x.ShoppingCartId,
                        principalTable: "ShoppingCartClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IDX_CLIENT",
                table: "ShoppingCartClients",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartItems_ShoppingCartId",
                table: "ShoppingCartItems",
                column: "ShoppingCartId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ShoppingCartItems");

            migrationBuilder.DropTable(
                name: "ShoppingCartClients");
        }
    }
}
