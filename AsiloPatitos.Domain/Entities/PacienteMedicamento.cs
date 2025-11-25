using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiloPatitos.Domain.Entities
{
    public class PacienteMedicamento
    {
        [Key]
        public int Id { get; set; }

        // Relaciones
        [Required]
        public int PacienteId { get; set; }

        [ForeignKey(nameof(PacienteId))]
        public Paciente Paciente { get; set; } = null!;

        [Required]
        public int MedicamentoId { get; set; }

        [ForeignKey(nameof(MedicamentoId))]
        public Medicamento Medicamento { get; set; } = null!;

        // Propiedades adicionales
        [Required]
        public DateTime FechaInicio { get; set; }

        [Required]
        public DateTime FechaFin { get; set; }

        public string? Indicaciones { get; set; }
    }
}
