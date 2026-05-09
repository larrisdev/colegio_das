using System.ComponentModel.DataAnnotations;

namespace Portal_Web_Colegio.Models
{
    public class Expediente
    {
        public int ExpedienteId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Elegí un estudiante de la lista.")]
        public int AlumnoId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Elegí una materia de la lista.")]
        public int MateriaId { get; set; }

        [Range(0, 10, ErrorMessage = "La calificación tiene que estar entre 0 y 10.")]
        [Display(Name = "Calificación final")]
        public float NotaFinal { get; set; }

        [Required(ErrorMessage = "Escribí las observaciones del docente.")]
        [Display(Name = "Observaciones")]
        public string Observaciones { get; set; } = string.Empty;

        public Alumno? Alumno { get; set; }

        public Materia? Materia { get; set; }
    }
}
