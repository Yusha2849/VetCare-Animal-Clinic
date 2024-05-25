using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetCare_Animal_Clinic.Data.Migrations
{
    public partial class AnimalType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AnimalTypes",
                columns: table => new
                {
                    AnimalType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Breed = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Species = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnimalTypes", x => new { x.AnimalType, x.Breed });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
