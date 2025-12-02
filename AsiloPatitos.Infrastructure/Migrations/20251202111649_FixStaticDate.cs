using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsiloPatitos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixStaticDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Empleados",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaIngreso",
                value: new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Empleados",
                keyColumn: "Id",
                keyValue: 1,
                column: "FechaIngreso",
                value: new DateTime(2023, 12, 2, 5, 15, 28, 123, DateTimeKind.Local).AddTicks(5532));
        }
    }
}
