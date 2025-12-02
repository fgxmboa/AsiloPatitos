using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiloPatitos.Domain.Entities
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }


        [Required(ErrorMessage = "Debe ingresar el nombre.")]
        [StringLength(100, ErrorMessage = "El nombre no debe superar los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe ingresar el apellido.")]
        [StringLength(100, ErrorMessage = "El apellido no debe superar los 100 caracteres.")]
        public string Apellido { get; set; } = string.Empty;



        [Required(ErrorMessage = "Debe ingresar un correo electrónico.")]
        [EmailAddress(ErrorMessage = "Debe ingresar un correo electrónico válido.")]
        [StringLength(150, ErrorMessage = "El correo no debe superar los 150 caracteres.")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe ingresar una contraseña.")]
        [StringLength(255, MinimumLength = 6,
            ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
        public string Contrasena { get; set; } = string.Empty;


        [Required(ErrorMessage = "Debe seleccionar un rol.")]
        [StringLength(50, ErrorMessage = "El rol no debe superar los 50 caracteres.")]
        public string Rol { get; set; } = "Empleado";

        public bool Activo { get; set; } = true;

        [Required]
        public DateTime FechaCreacion { get; set; } = DateTime.Now;


        public int? EmpleadoId { get; set; }

        [ForeignKey(nameof(EmpleadoId))]
        public Empleado? Empleado { get; set; }

        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpiracion { get; set; }

    }
}
