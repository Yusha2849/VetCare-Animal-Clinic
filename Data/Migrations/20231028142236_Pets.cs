using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VetCare_Animal_Clinic.Data.Migrations
{
    public partial class Pets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pet",
                columns: table => new
                {
                    PetId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PetName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AnimalType = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Breed = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    YearOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pet", x => x.PetId);
                    table.ForeignKey(
                        name: "FK_Pet_AnimalTypes_AnimalType_Breed",
                        columns: x => new { x.AnimalType, x.Breed },
                        principalTable: "AnimalTypes",
                        principalColumns: new[] { "AnimalType", "Breed" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pet_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pet_AnimalType_Breed",
                table: "Pet",
                columns: new[] { "AnimalType", "Breed" });

            migrationBuilder.CreateIndex(
                name: "IX_Pet_UserId",
                table: "Pet",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pet");
        }
    }
}
