using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiloPatitos.Domain.Entities
{
    public class Medicamento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Dosis { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Frecuencia { get; set; } = string.Empty;

        [Required]
        public int Stock { get; set; }
    }
}
