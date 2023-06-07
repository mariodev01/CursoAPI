using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CursoWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AgregandoDatos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenidad", "Detalle", "FechaActualizacion", "FechaCreacion", "ImagenUrl", "MetrosCuadrados", "Nombre", "Ocupantes", "Tarifa" },
                values: new object[,]
                {
                    { 1, "", "Detalle de la villa", new DateTime(2023, 6, 7, 12, 9, 14, 505, DateTimeKind.Local).AddTicks(1876), new DateTime(2023, 6, 7, 12, 9, 14, 505, DateTimeKind.Local).AddTicks(1865), "", 50, "Villa real", 5, 250.0 },
                    { 2, "", "Detalle de la villa 2", new DateTime(2023, 6, 7, 12, 9, 14, 505, DateTimeKind.Local).AddTicks(1879), new DateTime(2023, 6, 7, 12, 9, 14, 505, DateTimeKind.Local).AddTicks(1879), "", 80, "Villa real 2", 8, 280.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
