using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiloPatitos.Domain.Entities
{
    public class PacienteTratamiento
    {
        [Key]
        public int Id { get; set; }

        // Relaciones
        [Required]
        public int PacienteId { get; set; }

        [ForeignKey(nameof(PacienteId))]
        public Paciente Paciente { get; set; } = null!;

        [Required]
        public int TratamientoId { get; set; }

        [ForeignKey(nameof(TratamientoId))]
        public Tratamiento Tratamiento { get; set; } = null!;

        // Propiedades adicionales
        [Required]
        public DateTime FechaAplicacion { get; set; }

        public string? Observaciones { get; set; }
    }
}
