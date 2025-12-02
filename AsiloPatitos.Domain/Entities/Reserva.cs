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

        // Relaciones
        [Required(ErrorMessage = "Debe seleccionar un paciente.")]
        public int PacienteId { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una habitación.")]
        public int HabitacionId { get; set; }

        // Fechas
        [Required(ErrorMessage = "Debe ingresar la fecha de ingreso.")]
        [DataType(DataType.Date)]
        public DateTime FechaIngreso { get; set; }

        [Required(ErrorMessage = "Debe ingresar la fecha de salida.")]
        [DataType(DataType.Date)]
        [CompareDate(nameof(FechaIngreso), ErrorMessage = "La fecha de salida debe ser posterior a la fecha de ingreso.")]
        public DateTime FechaSalida { get; set; }

        // Tiempo de Estancia
        [Required(ErrorMessage = "Debe seleccionar un tiempo de estancia.")]
        [StringLength(30, ErrorMessage = "El tiempo de estancia no debe superar los 30 caracteres.")]
        public string TiempoEstancia { get; set; } = string.Empty;


        // Navegación
        public Paciente? Paciente { get; set; }
        public Habitacion? Habitacion { get; set; }
    }
}
