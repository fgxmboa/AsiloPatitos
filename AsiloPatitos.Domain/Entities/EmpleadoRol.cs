using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiloPatitos.Domain.Entities
{
    public class EmpleadoRol
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un empleado.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del empleado no es válido.")]
        public int EmpleadoId { get; set; }

        [ForeignKey(nameof(EmpleadoId))]
        public Empleado Empleado { get; set; } = null!;

        [Required(ErrorMessage = "Debe seleccionar un rol.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del rol no es válido.")]
        public int RolId { get; set; }

        [ForeignKey(nameof(RolId))]
        public Rol Rol { get; set; } = null!;
    }
}
