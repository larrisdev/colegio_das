using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portal_Web_Colegio.Models;

namespace Portal_Web_Colegio.Controllers
{
    public class MateriaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MateriaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Materias.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materia = await _context.Materias
                .FirstOrDefaultAsync(m => m.MateriaId == id);

            if (materia == null)
            {
                return NotFound();
            }

            return View(materia);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MateriaId,NombreMateria,Docente")] Materia materia)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(materia);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Quedó registrada la materia «{materia.NombreMateria}».";
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
                        "Algo falló al guardar la materia.");
                }
            }
            return View(materia);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materia = await _context.Materias.FindAsync(id);
            if (materia == null)
            {
                return NotFound();
            }
            return View(materia);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MateriaId,NombreMateria,Docente")] Materia materia)
        {
            if (id != materia.MateriaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(materia);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Se actualizó la materia «{materia.NombreMateria}».";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MateriaExists(materia.MateriaId))
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
                        "Algo falló al actualizar la materia.");
                }

                if (ModelState.IsValid)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(materia);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var materia = await _context.Materias
                .FirstOrDefaultAsync(m => m.MateriaId == id);

            if (materia == null)
            {
                return NotFound();
            }

            return View(materia);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var materia = await _context.Materias.FindAsync(id);
                if (materia != null)
                {
                    var etiqueta = materia.NombreMateria;
                    _context.Materias.Remove(materia);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"Se quitó la materia «{etiqueta}».";
                }
                else
                {
                    TempData["ErrorMessage"] = "No encontramos esa materia.";
                }
            }
            catch (DbUpdateException)
            {
                TempData["ErrorMessage"] =
                    "No se puede quitar porque tiene expedientes u otros datos ligados.";
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "Algo falló al quitar la materia.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool MateriaExists(int id)
        {
            return _context.Materias.Any(e => e.MateriaId == id);
        }
    }
}
