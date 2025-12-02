using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiloPatitos.Domain.Entities
{
    public class Empleado
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(120, ErrorMessage = "Máximo 120 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La cédula es obligatoria")]
        [StringLength(25, ErrorMessage = "Máximo 25 caracteres")]
        public string Cedula { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de ingreso es obligatoria")]
        [DataType(DataType.Date)]
        public DateTime FechaIngreso { get; set; }

        [Required(ErrorMessage = "El departamento es obligatorio")]
        [StringLength(60)]
        public string Departamento { get; set; } = string.Empty;

        [Required(ErrorMessage = "El perfil es obligatorio")]
        [StringLength(200)]
        public string Perfil { get; set; } = string.Empty;
    }

}
