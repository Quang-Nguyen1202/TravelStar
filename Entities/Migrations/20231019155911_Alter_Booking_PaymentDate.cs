using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelStar.Entities.Migrations
{
    public partial class Alter_Booking_PaymentDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "Booking",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "Booking");
        }
    }
}
