using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetCare_Animal_Clinic.Data.Migrations
{
    public partial class AppointmentUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserID",
                table: "Appointment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Appointment");
        }
    }
}
