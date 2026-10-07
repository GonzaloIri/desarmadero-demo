using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace proyecto_desarmado.Migrations
{
    /// <inheritdoc />
    public partial class AgregarDatosStockPiezas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Piezas_Vehiculos_VehiculoId",
                table: "Piezas");

            migrationBuilder.AlterColumn<int>(
                name: "VehiculoId",
                table: "Piezas",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "Categoria",
                table: "Piezas",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AnioOrigen",
                table: "Piezas",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Piezas",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Compatibilidades",
                table: "Piezas",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MarcaOrigen",
                table: "Piezas",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ModeloOrigen",
                table: "Piezas",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NumeroPrecinto",
                table: "Piezas",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumeroRudac",
                table: "Piezas",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PatenteOrigen",
                table: "Piezas",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Piezas_Vehiculos_VehiculoId",
                table: "Piezas",
                column: "VehiculoId",
                principalTable: "Vehiculos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Piezas_Vehiculos_VehiculoId",
                table: "Piezas");

            migrationBuilder.DropColumn(
                name: "AnioOrigen",
                table: "Piezas");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Piezas");

            migrationBuilder.DropColumn(
                name: "Compatibilidades",
                table: "Piezas");

            migrationBuilder.DropColumn(
                name: "MarcaOrigen",
                table: "Piezas");

            migrationBuilder.DropColumn(
                name: "ModeloOrigen",
                table: "Piezas");

            migrationBuilder.DropColumn(
                name: "NumeroPrecinto",
                table: "Piezas");

            migrationBuilder.DropColumn(
                name: "NumeroRudac",
                table: "Piezas");

            migrationBuilder.DropColumn(
                name: "PatenteOrigen",
                table: "Piezas");

            migrationBuilder.AlterColumn<int>(
                name: "VehiculoId",
                table: "Piezas",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Categoria",
                table: "Piezas",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_Piezas_Vehiculos_VehiculoId",
                table: "Piezas",
                column: "VehiculoId",
                principalTable: "Vehiculos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
