namespace Portal_Web_Colegio.Models
{
    public class EstadisticasViewModel
    {
        public List<string> EtiquetasPromedioPorAlumno { get; set; } = new();
        public List<double> ValoresPromedioPorAlumno { get; set; } = new();
        public List<string> EtiquetasDistribucionNotas { get; set; } = new();
        public List<int> CantidadesDistribucionNotas { get; set; } = new();
        public bool TieneDatos { get; set; }
        public int TotalExpedientes { get; set; }
    }
}
