using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiloPatitos.Domain.Entities
{
    public class Pago
    {
        [Key]
        public int Id { get; set; }

        // Relaciones
        [Required(ErrorMessage = "Debe seleccionar una reserva.")]
        public int ReservaId { get; set; }

        [ForeignKey(nameof(ReservaId))]
        public Reserva? Reserva { get; set; }

        // Propiedades
        [Required(ErrorMessage = "Debe ingresar la fecha de pago.")]
        public DateTime FechaPago { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Debe ingresar el monto.")]
        [Range(0.01, 999999.99, ErrorMessage = "El monto debe ser mayor a cero.")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Monto { get; set; }

        [Required(ErrorMessage = "Debe ingresar el método de pago.")]
        [StringLength(50, ErrorMessage = "El método no debe superar los 50 caracteres.")]
        public string Metodo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe ingresar el estado del pago.")]
        [StringLength(50, ErrorMessage = "El estado no debe superar los 50 caracteres.")]
        public string Estado { get; set; } = "Completado";
    }
}
