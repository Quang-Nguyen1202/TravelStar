using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelStar.Entities.Migrations
{
    public partial class Alter_Hotel_Room_Images : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "Room",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "Hotel",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Room");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Hotel");
        }
    }
}
