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

        [Required]
        public int ReservaId { get; set; }

        [ForeignKey(nameof(ReservaId))]
        public Reserva Reserva { get; set; } = null!;

        [Required]
        public DateTime FechaPago { get; set; } = DateTime.Now;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Monto { get; set; }

        [Required]
        [StringLength(50)]
        public string Metodo { get; set; } = string.Empty; 

        [Required]
        [StringLength(50)]
        public string Estado { get; set; } = "Completado"; 
    }
}
