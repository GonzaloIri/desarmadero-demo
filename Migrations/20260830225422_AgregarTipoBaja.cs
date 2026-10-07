using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace proyecto_desarmado.Migrations
{
    /// <inheritdoc />
    public partial class AgregarTipoBaja : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TipoBaja",
                table: "Vehiculos",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoBaja",
                table: "Vehiculos");
        }
    }
}
