using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiloPatitos.Domain.Entities
{
    public class Paciente
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del paciente es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "La cédula es obligatoria.")]
        [StringLength(50, ErrorMessage = "La cédula no puede superar los 50 caracteres.")]
        public string Cedula { get; set; } = null!;

        [Required(ErrorMessage = "Debe indicar la fecha de ingreso.")]
        [DataType(DataType.Date)]
        public DateTime FechaIngreso { get; set; }

        [StringLength(255, ErrorMessage = "El campo de medicamentos no puede superar los 255 caracteres.")]
        public string? Medicamentos { get; set; }

        [StringLength(255, ErrorMessage = "El campo de cuidados especiales no puede superar los 255 caracteres.")]
        public string? CuidadosEspeciales { get; set; }

        [Required(ErrorMessage = "Debe especificar el nivel de asistencia.")]
        [StringLength(100, ErrorMessage = "El nivel de asistencia no puede superar los 100 caracteres.")]
        public string NivelAsistencia { get; set; } = null!;

        [StringLength(100, ErrorMessage = "El campo de paquete adicional no puede superar los 100 caracteres.")]
        public string? PaqueteAdicional { get; set; }

        // Relación con Habitaciones
        public int? HabitacionId { get; set; }
        public Habitacion? Habitacion { get; set; }
    }
}
