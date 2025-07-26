using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sideas.Challenge.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class iunicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Agrupaciones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agrupaciones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Asignaciones",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroExp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AnioExp = table.Column<int>(type: "int", nullable: true),
                    Incidente = table.Column<int>(type: "int", nullable: true),
                    Autos = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NombreAuxiliar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipoDocAuxiliar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DocAuxiliar = table.Column<long>(type: "bigint", nullable: true),
                    CreacionFecha = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdProfesionAux = table.Column<int>(type: "int", nullable: true),
                    EspecialidadAuxiliar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfesionAuxiliar = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fuero = table.Column<int>(type: "int", nullable: true),
                    Zona = table.Column<int>(type: "int", nullable: true),
                    Reparticion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asignaciones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Fueros",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fueros", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Profesiones",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ProfesionCodigo = table.Column<int>(type: "int", nullable: false),
                    Especialidad = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profesiones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Zonas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Reparticion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FueroId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zonas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AgrupacionProfesiones",
                columns: table => new
                {
                    AgrupacionId = table.Column<int>(type: "int", nullable: false),
                    ProfesionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgrupacionProfesiones", x => new { x.AgrupacionId, x.ProfesionId });
                    table.ForeignKey(
                        name: "FK_AgrupacionProfesiones_Agrupaciones_AgrupacionId",
                        column: x => x.AgrupacionId,
                        principalTable: "Agrupaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AgrupacionProfesiones_Profesiones_ProfesionId",
                        column: x => x.ProfesionId,
                        principalTable: "Profesiones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AgrupacionProfesiones_ProfesionId",
                table: "AgrupacionProfesiones",
                column: "ProfesionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AgrupacionProfesiones");

            migrationBuilder.DropTable(
                name: "Asignaciones");

            migrationBuilder.DropTable(
                name: "Fueros");

            migrationBuilder.DropTable(
                name: "Zonas");

            migrationBuilder.DropTable(
                name: "Agrupaciones");

            migrationBuilder.DropTable(
                name: "Profesiones");
        }
    }
}
