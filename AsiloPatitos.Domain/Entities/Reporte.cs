using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AsiloPatitos.Domain.Entities
{
    public class Reporte
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int EmpleadoId { get; set; }

        [Required]
        public int PacienteId { get; set; }

        [Required]
        [StringLength(200)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public string Tipo { get; set; } = "General";

        [ForeignKey(nameof(EmpleadoId))]
        public Empleado? Empleado { get; set; }

        [ForeignKey(nameof(PacienteId))]
        public Paciente? Paciente { get; set; }
    }
}
