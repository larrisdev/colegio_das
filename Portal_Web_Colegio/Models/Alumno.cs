using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Portal_Web_Colegio.Models
{
    public class Alumno
    {
        public int AlumnoId { get; set; }

        [Required(ErrorMessage = "Tenés que escribir el nombre.")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tenés que escribir el apellido.")]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "Indicá la fecha de nacimiento.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de nacimiento")]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "Indicá el grado o sección.")]
        [Display(Name = "Grado")]
        public string Grado { get; set; } = string.Empty;

        public ICollection<Expediente>? Expedientes { get; set; }
    }
}
