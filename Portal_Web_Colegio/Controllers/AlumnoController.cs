using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portal_Web_Colegio.Models;

namespace Portal_Web_Colegio.Controllers
{
    public class AlumnoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AlumnoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Alumnos.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alumno = await _context.Alumnos
                .FirstOrDefaultAsync(a => a.AlumnoId == id);

            if (alumno == null)
            {
                return NotFound();
            }

            return View(alumno);
        }

        public IActionResult Create()
        {
            return View(new Alumno { FechaNacimiento = DateTime.Today });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Apellido,FechaNacimiento,Grado")] Alumno alumno)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(alumno);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Quedó registrado {alumno.Nombre} {alumno.Apellido}.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty,
                        "No se pudo guardar la información. Revisá la conexión e intentá de nuevo.");
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty,
                        "Algo falló al guardar. Intentá de nuevo más tarde.");
                }
            }

            return View(alumno);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alumno = await _context.Alumnos.FindAsync(id);
            if (alumno == null)
            {
                return NotFound();
            }
            return View(alumno);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AlumnoId,Nombre,Apellido,FechaNacimiento,Grado")] Alumno alumno)
        {
            if (id != alumno.AlumnoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(alumno);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Se actualizaron los datos de {alumno.Nombre} {alumno.Apellido}.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlumnoExists(alumno.AlumnoId))
                    {
                        return NotFound();
                    }

                    ModelState.AddModelError(string.Empty,
                        "Otro usuario cambió este registro. Actualizá la página e intentá de nuevo.");
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty,
                        "No se pudieron guardar los cambios. Revisá la conexión e intentá de nuevo.");
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty,
                        "Algo falló al actualizar los datos.");
                }
            }

            return View(alumno);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alumno = await _context.Alumnos
                .FirstOrDefaultAsync(a => a.AlumnoId == id);

            if (alumno == null)
            {
                return NotFound();
            }

            return View(alumno);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var alumno = await _context.Alumnos.FindAsync(id);
                if (alumno != null)
                {
                    var etiqueta = $"{alumno.Nombre} {alumno.Apellido}";
                    _context.Alumnos.Remove(alumno);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Se dio de baja a {etiqueta}.";
                }
                else
                {
                    TempData["ErrorMessage"] = "No encontramos ese estudiante.";
                }
            }
            catch (DbUpdateException)
            {
                TempData["ErrorMessage"] =
                    "No se puede dar de baja porque tiene expedientes u otros datos ligados.";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Algo falló al dar de baja al estudiante.";
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> PromedioNotas()
        {
            var porAlumno = await _context.Expedientes
                .GroupBy(e => e.AlumnoId)
                .Select(g => new { g.Key, Promedio = g.Average(e => (double)e.NotaFinal) })
                .ToListAsync();

            var ids = porAlumno.Select(x => x.Key).ToList();
            var alumnos = await _context.Alumnos
                .Where(a => ids.Contains(a.AlumnoId))
                .ToListAsync();
            var porId = alumnos.ToDictionary(a => a.AlumnoId);

            var modelo = porAlumno
                .Select(x => new PromedioNotaRowViewModel
                {
                    Alumno = porId.GetValueOrDefault(x.Key),
                    Promedio = x.Promedio
                })
                .Where(r => r.Alumno != null)
                .OrderBy(r => r.Alumno!.Apellido)
                .ThenBy(r => r.Alumno!.Nombre)
                .ToList();

            return View(modelo);
        }

        private bool AlumnoExists(int id)
        {
            return _context.Alumnos.Any(e => e.AlumnoId == id);
        }
    }
}
