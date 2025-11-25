using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsiloPatitos.Domain.Entities
{
        public class PacienteMedicamento
    {
        [Key]
        public int Id { get; set; }

        // Relaciones
        [Required(ErrorMessage = "Debe seleccionar un paciente.")]
        public int PacienteId { get; set; }

        [ForeignKey(nameof(PacienteId))]
        public Paciente? Paciente { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un medicamento.")]
        public int MedicamentoId { get; set; }

        [ForeignKey(nameof(MedicamentoId))]
        public Medicamento? Medicamento { get; set; }

        // Propiedades adicionales
        [Required(ErrorMessage = "Debe ingresar la fecha de inicio.")]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "Debe ingresar la fecha de fin.")]
        [DataType(DataType.Date)]
        [CompareDate(nameof(FechaInicio), ErrorMessage = "La fecha de fin debe ser posterior a la fecha de inicio.")]
        public DateTime FechaFin { get; set; }

        [StringLength(200, ErrorMessage = "Las indicaciones no deben superar los 200 caracteres.")]
        public string? Indicaciones { get; set; }
    }

    // Validación personalizada
    public class CompareDateAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public CompareDateAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var currentValue = (DateTime?)value;
            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
            if (property == null) return new ValidationResult($"Propiedad {_comparisonProperty} no encontrada.");

            var comparisonValue = (DateTime?)property.GetValue(validationContext.ObjectInstance);

            if (currentValue.HasValue && comparisonValue.HasValue && currentValue <= comparisonValue)
                return new ValidationResult(ErrorMessage);

            return ValidationResult.Success;
        }
    }
}
