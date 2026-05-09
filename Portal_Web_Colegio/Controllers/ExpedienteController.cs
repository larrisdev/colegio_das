using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Portal_Web_Colegio.Models;

namespace Portal_Web_Colegio.Controllers
{
    public class ExpedienteController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExpedienteController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var expedientes = _context.Expedientes
                .Include(e => e.Alumno)
                .Include(e => e.Materia);

            return View(await expedientes.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expediente = await _context.Expedientes
                .Include(e => e.Alumno)
                .Include(e => e.Materia)
                .FirstOrDefaultAsync(e => e.ExpedienteId == id);

            if (expediente == null)
            {
                return NotFound();
            }

            return View(expediente);
        }

        public IActionResult Create()
        {
            PopulateDropdowns();
            if (!_context.Alumnos.Any() || !_context.Materias.Any())
            {
                TempData["ErrorMessage"] = "Primero tenés que tener estudiantes y materias registradas para crear expedientes.";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExpedienteId,AlumnoId,MateriaId,NotaFinal,Observaciones")] Expediente expediente)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(expediente);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Quedó guardado el expediente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty,
                        "No se pudo guardar el expediente. Revisá los datos e intentá de nuevo.");
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty,
                        "Algo falló al guardar el expediente.");
                }
            }

            PopulateDropdowns(expediente.AlumnoId, expediente.MateriaId);
            return View(expediente);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expediente = await _context.Expedientes.FindAsync(id);
            if (expediente == null)
            {
                return NotFound();
            }

            PopulateDropdowns(expediente.AlumnoId, expediente.MateriaId);
            return View(expediente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExpedienteId,AlumnoId,MateriaId,NotaFinal,Observaciones")] Expediente expediente)
        {
            if (id != expediente.ExpedienteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(expediente);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Se actualizó el expediente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpedienteExists(expediente.ExpedienteId))
                    {
                        return NotFound();
                    }
                    ModelState.AddModelError(string.Empty,
                        "Otro usuario cambió este registro. Actualizá la página e intentá de nuevo.");
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError(string.Empty,
                        "No se pudieron guardar los cambios.");
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty,
                        "Algo falló al actualizar el expediente.");
                }

                if (ModelState.IsValid)
                {
                    return RedirectToAction(nameof(Index));
                }
            }

            PopulateDropdowns(expediente.AlumnoId, expediente.MateriaId);
            return View(expediente);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expediente = await _context.Expedientes
                .Include(e => e.Alumno)
                .Include(e => e.Materia)
                .FirstOrDefaultAsync(e => e.ExpedienteId == id);

            if (expediente == null)
            {
                return NotFound();
            }

            return View(expediente);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var expediente = await _context.Expedientes.FindAsync(id);
                if (expediente != null)
                {
                    _context.Expedientes.Remove(expediente);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Se quitó el expediente.";
                }
                else
                {
                    TempData["ErrorMessage"] = "No encontramos ese expediente.";
                }
            }
            catch (DbUpdateException)
            {
                TempData["ErrorMessage"] = "No se pudo quitar el expediente.";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Algo falló al quitar el expediente.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ExpedienteExists(int id)
        {
            return _context.Expedientes.Any(e => e.ExpedienteId == id);
        }

        private void PopulateDropdowns(object? selectedAlumno = null, object? selectedMateria = null)
        {
            var alumnos = _context.Alumnos
                .OrderBy(a => a.Apellido).ThenBy(a => a.Nombre)
                .Select(a => new { a.AlumnoId, NombreCompleto = a.Apellido + ", " + a.Nombre })
                .ToList();

            ViewData["AlumnoId"] = new SelectList(alumnos, "AlumnoId", "NombreCompleto", selectedAlumno);

            var materias = _context.Materias.OrderBy(m => m.NombreMateria).ToList();
            ViewData["MateriaId"] = new SelectList(materias, "MateriaId", "NombreMateria", selectedMateria);
        }
    }
}
