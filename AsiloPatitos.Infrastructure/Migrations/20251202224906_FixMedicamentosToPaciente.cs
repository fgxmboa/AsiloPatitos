using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AsiloPatitos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixMedicamentosToPaciente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pacientes_Medicamentos_MedicamentoId",
                table: "Pacientes");

            migrationBuilder.DropIndex(
                name: "IX_Pacientes_MedicamentoId",
                table: "Pacientes");

            migrationBuilder.DropColumn(
                name: "MedicamentoId",
                table: "Pacientes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.AddForeignKey(
                name: "FK_Pacientes_Medicamentos_MedicamentoId",
                table: "Pacientes",
                column: "MedicamentoId",
                principalTable: "Medicamentos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
