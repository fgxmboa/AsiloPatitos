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
        [Required(ErrorMessage = "Debe seleccionar un paciente.")]
        public int PacienteId { get; set; }

        [ForeignKey(nameof(PacienteId))]
        public Paciente? Paciente { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un tratamiento.")]
        public int TratamientoId { get; set; }

        [ForeignKey(nameof(TratamientoId))]
        public Tratamiento? Tratamiento { get; set; }

        // Propiedades adicionales
        [Required(ErrorMessage = "Debe ingresar la fecha de aplicación.")]
        [DataType(DataType.Date)]
        public DateTime FechaAplicacion { get; set; }

        [StringLength(300, ErrorMessage = "Las observaciones no deben superar los 300 caracteres.")]
        public string? Observaciones { get; set; }
    }
}
