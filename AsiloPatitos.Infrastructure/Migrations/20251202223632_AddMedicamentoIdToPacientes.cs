using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsiloPatitos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMedicamentoIdToPacientes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PacienteMedicamentos_Medicamentos_MedicamentoId",
                table: "PacienteMedicamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_Pacientes_Medicamentos_MedicamentoId",
                table: "Pacientes");

            migrationBuilder.AddForeignKey(
                name: "FK_PacienteMedicamentos_Medicamentos_MedicamentoId",
                table: "PacienteMedicamentos",
                column: "MedicamentoId",
                principalTable: "Medicamentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pacientes_Medicamentos_MedicamentoId",
                table: "Pacientes",
                column: "MedicamentoId",
                principalTable: "Medicamentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PacienteMedicamentos_Medicamentos_MedicamentoId",
                table: "PacienteMedicamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_Pacientes_Medicamentos_MedicamentoId",
                table: "Pacientes");

            migrationBuilder.AddForeignKey(
                name: "FK_PacienteMedicamentos_Medicamentos_MedicamentoId",
                table: "PacienteMedicamentos",
                column: "MedicamentoId",
                principalTable: "Medicamentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pacientes_Medicamentos_MedicamentoId",
                table: "Pacientes",
                column: "MedicamentoId",
                principalTable: "Medicamentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
