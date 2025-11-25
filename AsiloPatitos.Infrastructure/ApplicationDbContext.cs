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
        }
    }
}
