using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace proyecto_desarmado.Migrations
{
    /// <inheritdoc />
    public partial class AgregarRegistroImportacionesStock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ImportacionesStock",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    NombreArchivo = table.Column<string>(type: "TEXT", nullable: false),
                    HuellaArchivo = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    FechaUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CantidadPiezas = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportacionesStock", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImportacionesStock_HuellaArchivo",
                table: "ImportacionesStock",
                column: "HuellaArchivo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ImportacionesStock");
        }
    }
}
