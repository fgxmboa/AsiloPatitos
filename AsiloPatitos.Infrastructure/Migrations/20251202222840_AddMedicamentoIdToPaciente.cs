using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsiloPatitos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMedicamentoIdToPaciente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Habitaciones_PacienteId",
                table: "Habitaciones");

            migrationBuilder.DropColumn(
                name: "HabitacionId",
                table: "Pacientes");

            migrationBuilder.AddColumn<int>(
                name: "MedicamentoId",
                table: "Pacientes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Pacientes_MedicamentoId",
                table: "Pacientes",
                column: "MedicamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Habitaciones_PacienteId",
                table: "Habitaciones",
                column: "PacienteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pacientes_Medicamentos_MedicamentoId",
                table: "Pacientes",
                column: "MedicamentoId",
                principalTable: "Medicamentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pacientes_Medicamentos_MedicamentoId",
                table: "Pacientes");

            migrationBuilder.DropIndex(
                name: "IX_Pacientes_MedicamentoId",
                table: "Pacientes");

            migrationBuilder.DropIndex(
                name: "IX_Habitaciones_PacienteId",
                table: "Habitaciones");

            migrationBuilder.DropColumn(
                name: "MedicamentoId",
                table: "Pacientes");

            migrationBuilder.AddColumn<int>(
                name: "HabitacionId",
                table: "Pacientes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Habitaciones_PacienteId",
                table: "Habitaciones",
                column: "PacienteId",
                unique: true,
                filter: "[PacienteId] IS NOT NULL");
        }
    }
}
