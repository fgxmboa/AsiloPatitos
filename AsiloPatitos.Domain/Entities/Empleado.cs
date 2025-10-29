using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiloPatitos.Domain.Entities
{
    public class Empleado
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Cedula { get; set; } = null!;
        public DateTime FechaIngreso { get; set; }
        public string Departamento { get; set; } = null!;
        public string Perfil { get; set; } = null!;
    }

}
