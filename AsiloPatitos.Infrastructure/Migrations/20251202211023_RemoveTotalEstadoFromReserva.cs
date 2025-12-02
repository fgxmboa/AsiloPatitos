using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsiloPatitos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTotalEstadoFromReserva : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "Reservas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Reservas",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Total",
                table: "Reservas",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
