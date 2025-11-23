using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AsiloPatitos.Domain.Entities
{
    public class Reserva
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PacienteId { get; set; }

        [Required]
        public int HabitacionId { get; set; }

        [Required]
        public DateTime FechaIngreso { get; set; }

        [Required]
        public DateTime FechaSalida { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Total { get; set; }

        public string Estado { get; set; } = "Pendiente";

        // Relaciones
        [ForeignKey(nameof(PacienteId))]
        public Paciente? Paciente { get; set; }

        [ForeignKey(nameof(HabitacionId))]
        public Habitacion? Habitacion { get; set; }
    }
}
