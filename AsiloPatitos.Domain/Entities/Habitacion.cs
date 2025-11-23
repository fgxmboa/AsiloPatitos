using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiloPatitos.Domain.Entities
{
    public class Habitacion
    {
        public int Id { get; set; }
        public string Numero { get; set; } = null!;
        public string Tipo { get; set; } = null!;
        public bool Disponible { get; set; }
        public decimal PrecioPorDia { get; set; }

        // Relación: una habitación puede estar asignada a un paciente
        public int? PacienteId { get; set; }
        public Paciente? Paciente { get; set; }
    }
}
