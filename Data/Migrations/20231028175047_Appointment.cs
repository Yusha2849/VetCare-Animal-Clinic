using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetCare_Animal_Clinic.Data.Migrations
{
    public partial class Appointment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Appointment",
                columns: table => new
                {
                    AppointmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ADate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ATime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PetID = table.Column<int>(type: "int", nullable: false),
                    Appointment_Reason = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AppointmentStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointment", x => x.AppointmentID);
                    table.ForeignKey(
                        name: "FK_Appointment_Pet_PetID",
                        column: x => x.PetID,
                        principalTable: "Pet",
                        principalColumn: "PetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointment_PetID",
                table: "Appointment",
                column: "PetID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointment");
        }
    }
}
