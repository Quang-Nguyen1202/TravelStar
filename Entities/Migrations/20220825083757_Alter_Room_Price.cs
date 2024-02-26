using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelStar.Entities.Migrations
{
    public partial class Alter_Room_Price : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Price",
                table: "Room",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Room");
        }
    }
}
