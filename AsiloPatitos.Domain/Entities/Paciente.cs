using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiloPatitos.Domain.Entities
{
    public class Paciente
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Cedula { get; set; } = null!;
        public DateTime FechaIngreso { get; set; }
        public string Medicamentos { get; set; } = null!;
        public string CuidadosEspeciales { get; set; } = null!;
        public string NivelAsistencia { get; set; } = null!;
        public string PaqueteAdicional { get; set; } = null!;
    }
}
