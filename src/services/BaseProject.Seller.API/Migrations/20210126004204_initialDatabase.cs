using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BaseProject.Seller.API.Migrations
{
    public partial class initialDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Seller",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DocRef = table.Column<string>(type: "varchar(30)", nullable: true),
                    DocType = table.Column<string>(type: "varchar(30)", nullable: true),
                    Name = table.Column<string>(type: "varchar(100)", nullable: false),
                    Address = table.Column<string>(type: "varchar(200)", nullable: true),
                    Neighbohood = table.Column<string>(type: "varchar(30)", nullable: true),
                    Number = table.Column<string>(type: "varchar(10)", nullable: true),
                    City = table.Column<string>(type: "varchar(200)", nullable: true),
                    Reference = table.Column<string>(type: "varchar(150)", nullable: true),
                    WorkingTime = table.Column<string>(type: "varchar(100)", nullable: true),
                    Details = table.Column<string>(type: "varchar(2000)", nullable: true),
                    Image = table.Column<string>(type: "varchar(200)", nullable: true),
                    HasParking = table.Column<bool>(type: "boolean", nullable: false),
                    HasDelivery = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seller", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Seller");
        }
    }
}
