using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Portal_Web_Colegio.Models
{
    public class Materia
    {
        public int MateriaId { get; set; }
        [Required(ErrorMessage = "Tenés que escribir el nombre de la materia.")]
        [Display(Name = "Nombre de la materia")]
        public string NombreMateria { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tenés que escribir el nombre del docente.")]
        [Display(Name = "Docente")]
        public string Docente { get; set; } = string.Empty;

        public ICollection<Expediente> Expedientes { get; set; } = new List<Expediente>();
    }
}
