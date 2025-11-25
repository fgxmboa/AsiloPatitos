using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiloPatitos.Domain.Entities
{
    public class Habitacion
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El número de habitación es obligatorio.")]
        [StringLength(20, ErrorMessage = "El número no puede superar los 20 caracteres.")]
        public string Numero { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe especificar el tipo de habitación.")]
        [StringLength(50, ErrorMessage = "El tipo no puede superar los 50 caracteres.")]
        public string Tipo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe indicar si la habitación está disponible.")]
        public bool Disponible { get; set; }

        [Required(ErrorMessage = "Debe especificar el precio por día.")]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0.01, 999999.99, ErrorMessage = "El precio por día debe ser mayor a 0.")]
        public decimal PrecioPorDia { get; set; }

        // Relación: una habitación puede estar asignada a un paciente
        public int? PacienteId { get; set; }

        [ForeignKey(nameof(PacienteId))]
        public Paciente? Paciente { get; set; }
    }
}
