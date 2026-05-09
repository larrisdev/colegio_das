using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portal_Web_Colegio.Models;

namespace Portal_Web_Colegio.Controllers
{
    public class EstadisticasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EstadisticasController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var total = await _context.Expedientes.CountAsync();
            var vm = new EstadisticasViewModel
            {
                TieneDatos = total > 0,
                TotalExpedientes = total
            };

            if (!vm.TieneDatos)
            {
                return View(vm);
            }

            var grupos = await _context.Expedientes
                .GroupBy(e => e.AlumnoId)
                .Select(g => new { g.Key, Promedio = g.Average(e => (double)e.NotaFinal) })
                .ToListAsync();

            var ids = grupos.Select(g => g.Key).ToList();
            var alumnos = await _context.Alumnos
                .Where(a => ids.Contains(a.AlumnoId))
                .ToDictionaryAsync(a => a.AlumnoId);

            var ordenados = grupos
                .OrderByDescending(g => g.Promedio)
                .ToList();

            foreach (var g in ordenados)
            {
                var etiqueta = alumnos.TryGetValue(g.Key, out var al)
                    ? $"{al.Apellido}, {al.Nombre}"
                    : $"Alumno #{g.Key}";
                vm.EtiquetasPromedioPorAlumno.Add(etiqueta);
                vm.ValoresPromedioPorAlumno.Add(Math.Round(g.Promedio, 2));
            }

            var notas = await _context.Expedientes
                .Select(e => (double)e.NotaFinal)
                .ToListAsync();

            vm.EtiquetasDistribucionNotas.AddRange(new[]
            {
                "Insuficiente (<6)",
                "Regular (6–6.9)",
                "Bueno (7–7.9)",
                "Muy bueno (8–8.9)",
                "Excelente (≥9)"
            });

            vm.CantidadesDistribucionNotas.Add(notas.Count(n => n < 6));
            vm.CantidadesDistribucionNotas.Add(notas.Count(n => n >= 6 && n < 7));
            vm.CantidadesDistribucionNotas.Add(notas.Count(n => n >= 7 && n < 8));
            vm.CantidadesDistribucionNotas.Add(notas.Count(n => n >= 8 && n < 9));
            vm.CantidadesDistribucionNotas.Add(notas.Count(n => n >= 9));

            return View(vm);
        }
    }
}
