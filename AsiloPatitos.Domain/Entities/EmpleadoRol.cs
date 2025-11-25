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

        [Required]
        public int EmpleadoId { get; set; }

        [ForeignKey(nameof(EmpleadoId))]
        public Empleado Empleado { get; set; } = null!;

        [Required]
        public int RolId { get; set; }

        [ForeignKey(nameof(RolId))]
        public Rol Rol { get; set; } = null!;
    }
}
