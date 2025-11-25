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

        [Required(ErrorMessage = "El nombre del medicamento es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres.")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "Debe indicar la dosis del medicamento.")]
        [StringLength(100, ErrorMessage = "La dosis no puede tener más de 100 caracteres.")]
        public string Dosis { get; set; } = null!;

        [Required(ErrorMessage = "Debe especificar la frecuencia del medicamento.")]
        [StringLength(100, ErrorMessage = "La frecuencia no puede tener más de 100 caracteres.")]
        public string Frecuencia { get; set; } = null!;

        [Required(ErrorMessage = "Debe indicar la cantidad disponible en stock.")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock debe ser un número positivo.")]
        public int Stock { get; set; }

        // Relación: un medicamento puede estar asignado a varios pacientes
        public ICollection<PacienteMedicamento>? PacienteMedicamentos { get; set; }
    }
}
