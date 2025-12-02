using AsiloPatitos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiloPatitos.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) 
        { 
        }

        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Habitacion> Habitaciones { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Reporte> Reportes { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Tratamiento> Tratamientos { get; set; }
        public DbSet<PacienteTratamiento> PacienteTratamientos { get; set; }
        public DbSet<Medicamento> Medicamentos { get; set; }
        public DbSet<PacienteMedicamento> PacienteMedicamentos { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<EmpleadoRol> EmpleadoRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =====================
            //  ROLES
            // =====================
            modelBuilder.Entity<Rol>().HasData(
                new Rol { Id = 1, Nombre = "Administrador", Descripcion = "Acceso total" },
                new Rol { Id = 2, Nombre = "Gerencia", Descripcion = "Acceso a módulo de Registro de Empleados, Pacientes, Habitaciones, Reservas" },
                new Rol { Id = 3, Nombre = "Gestión de Pacientes", Descripcion = "Acceso a módulo de Pacientes y Habitaciones" },
                new Rol { Id = 4, Nombre = "Mantenimiento", Descripcion = "Acceso únicamente a Habitaciones" },
                new Rol { Id = 5, Nombre = "Recepción", Descripcion = "Acceso únicamente a Reservas" }
            );

            // =====================
            //  HABITACIONES
            // =====================
            modelBuilder.Entity<Habitacion>().HasData(
                new Habitacion { Id = 1, Numero = "101", Tipo = "Individual", Disponible = true, PrecioPorDia = 25000 },
                new Habitacion { Id = 2, Numero = "102", Tipo = "Individual", Disponible = true, PrecioPorDia = 25000 },
                new Habitacion { Id = 3, Numero = "201", Tipo = "Compartida", Disponible = true, PrecioPorDia = 18000 },
                new Habitacion { Id = 4, Numero = "202", Tipo = "Compartida", Disponible = true, PrecioPorDia = 18000 },
                new Habitacion { Id = 5, Numero = "301", Tipo = "Matrimonial", Disponible = true, PrecioPorDia = 30000 }
            );

            // =====================
            //  EMPLEADO BASE
            // =====================
            modelBuilder.Entity<Empleado>().HasData(
                new Empleado
                {
                    Id = 1,
                    Nombre = "Administrador",
                    Cedula = "1-1111-1111",
                    FechaIngreso = new DateTime(2025, 01, 01),
                    Departamento = "Administración",
                    Perfil = "Encargado de la gestión general"                    
                }
            );

            // =====================
            //  USUARIO ADMINISTRADOR BASE
            // =====================
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    Id = 1,
                    Nombre = "Admin",
                    Apellido = "Sistema",
                    Correo = "admin@patitos.com",
                    Contrasena = "$2a$11$M9vB7pSGH2z0qK/Cgmi4v.N/0tXk9N6yjifZEB6Tt0z9oEXzEOmQ2",
                    Rol = "Administrador",
                    Activo = true,
                    FechaCreacion = new DateTime(2025, 01, 01),
                    EmpleadoId = 1
                }
            );

            // =====================
            //  TRATAMIENTOS
            // =====================
            modelBuilder.Entity<Tratamiento>().HasData(
                new Tratamiento { Id = 1, Nombre = "Fisioterapia", Descripcion = "Sesiones terapéuticas básicas", Costo = 15000 },
                new Tratamiento { Id = 2, Nombre = "Control médico", Descripcion = "Consulta y seguimiento", Costo = 12000 },
                new Tratamiento { Id = 3, Nombre = "Cuidado especial", Descripcion = "Atención especializada diaria", Costo = 20000 }
            );

            // =====================
            //  MEDICAMENTOS
            // =====================
            modelBuilder.Entity<Medicamento>().HasData(
                new Medicamento { Id = 1, Nombre = "Paracetamol", Dosis = "500mg cada 8h", Frecuencia = "8 horas", Stock = 800 },
                new Medicamento { Id = 2, Nombre = "Ibuprofeno", Dosis = "400mg cada 8h", Frecuencia = "8 horas", Stock =  214 },
                new Medicamento { Id = 3, Nombre = "Omeprazol", Dosis = "20mg en ayunas", Frecuencia = "8 horas", Stock =  913 }
            );

            // Índice único por cédula para evitar duplicados
            modelBuilder.Entity<Empleado>()
                .HasIndex(e => e.Cedula)
                .IsUnique();

            modelBuilder.Entity<Habitacion>()
                .Property(h => h.PrecioPorDia)
                .HasPrecision(10, 2);

            modelBuilder.Entity<EmpleadoRol>()
                .HasIndex(er => new { er.EmpleadoId, er.RolId })
                .IsUnique();

            modelBuilder.Entity<Rol>()
                .HasIndex(r => r.Nombre)
                .IsUnique();
        }
    }
}
