using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PilotMaster.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddManobraCostFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Navios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bandeira = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataConstrucao = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Navios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SenhaHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Manobras",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    GRT = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Area = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Calado = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SemMaquinaLeme = table.Column<bool>(type: "bit", nullable: false),
                    Emergencia = table.Column<bool>(type: "bit", nullable: false),
                    NavioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manobras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Manobras_Navios_NavioId",
                        column: x => x.NavioId,
                        principalTable: "Navios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Manobras_NavioId",
                table: "Manobras",
                column: "NavioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Manobras");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Navios");
        }
    }
}
