using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AsiloPatitos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Empleados",
                columns: new[] { "Id", "Cedula", "Departamento", "FechaIngreso", "Nombre", "Perfil" },
                values: new object[] { 1, "1-1111-1111", "Administración", new DateTime(2023, 12, 2, 5, 12, 43, 403, DateTimeKind.Local).AddTicks(830), "Administrador", "Encargado de la gestión general del asilo" });

            migrationBuilder.InsertData(
                table: "Habitaciones",
                columns: new[] { "Id", "Disponible", "Numero", "PacienteId", "PrecioPorDia", "Tipo" },
                values: new object[,]
                {
                    { 1, true, "101", null, 25000m, "Individual" },
                    { 2, true, "102", null, 25000m, "Individual" },
                    { 3, true, "201", null, 18000m, "Compartida" },
                    { 4, true, "202", null, 18000m, "Compartida" },
                    { 5, true, "301", null, 30000m, "Matrimonial" }
                });

            migrationBuilder.InsertData(
                table: "Medicamentos",
                columns: new[] { "Id", "Dosis", "Frecuencia", "Nombre", "Stock" },
                values: new object[,]
                {
                    { 1, "500mg cada 8h", "8 horas", "Paracetamol", 800 },
                    { 2, "400mg cada 8h", "8 horas", "Ibuprofeno", 214 },
                    { 3, "20mg en ayunas", "8 horas", "Omeprazol", 913 }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { 1, "Acceso total", "Administrador" },
                    { 2, "Acceso a módulo de Registro de Empleados, Pacientes, Habitaciones, Reservas", "Gerencia" },
                    { 3, "Acceso a módulo de Pacientes y Habitaciones", "Gestión de Pacientes" },
                    { 4, "Acceso únicamente a Habitaciones", "Mantenimiento" },
                    { 5, "Acceso únicamente a Reservas", "Recepción" }
                });

            migrationBuilder.InsertData(
                table: "Tratamientos",
                columns: new[] { "Id", "Costo", "Descripcion", "Nombre" },
                values: new object[,]
                {
                    { 1, 15000m, "Sesiones terapéuticas básicas", "Fisioterapia" },
                    { 2, 12000m, "Consulta y seguimiento", "Control médico" },
                    { 3, 20000m, "Atención especializada diaria", "Cuidado especial" }
                });

            migrationBuilder.InsertData(
                table: "Usuarios",
                columns: new[] { "Id", "Activo", "Apellido", "Contrasena", "Correo", "EmpleadoId", "FechaCreacion", "Nombre", "ResetToken", "ResetTokenExpiracion", "Rol" },
                values: new object[] { 1, true, "Sistema", "$2a$11$M9vB7pSGH2z0qK/Cgmi4v.N/0tXk9N6yjifZEB6Tt0z9oEXzEOmQ2", "admin@patitos.com", 1, new DateTime(2025, 12, 2, 5, 12, 43, 404, DateTimeKind.Local).AddTicks(7648), "Admin", null, null, "Administrador" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Habitaciones",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Habitaciones",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Habitaciones",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Habitaciones",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Habitaciones",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Medicamentos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Medicamentos",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Medicamentos",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Tratamientos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Tratamientos",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Tratamientos",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Usuarios",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Empleados",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
