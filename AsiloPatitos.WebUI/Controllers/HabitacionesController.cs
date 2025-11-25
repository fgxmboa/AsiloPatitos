using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AsiloPatitos.Domain.Entities;
using AsiloPatitos.Infrastructure;

namespace AsiloPatitos.WebUI.Controllers
{
    public class HabitacionesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HabitacionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Habitaciones
        public async Task<IActionResult> Index()
        {
            var habitaciones = await _context.Habitaciones
                .Include(h => h.Paciente)
                .ToListAsync();
            return View(habitaciones);
        }

        // GET: Habitaciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "No se encontró la habitación solicitada.";
                return RedirectToAction(nameof(Index));
            }

            var habitacion = await _context.Habitaciones
                .Include(h => h.Paciente)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (habitacion == null)
            {
                TempData["ErrorMessage"] = "La habitación no existe.";
                return RedirectToAction(nameof(Index));
            }

            return View(habitacion);
        }

        // GET: Habitaciones/Create
        public IActionResult Create()
        {
            TempData.Remove("SuccessMessage");
            TempData.Remove("ErrorMessage");
            return View();
        }

        // POST: Habitaciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Habitacion habitacion)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Por favor, complete todos los campos requeridos.";
                return View(habitacion);
            }

            try
            {
                bool existe = await _context.Habitaciones.AnyAsync(h => h.Numero == habitacion.Numero);
                if (existe)
                {
                    TempData["ErrorMessage"] = "Ya existe una habitación con ese número.";
                    return View(habitacion);
                }

                _context.Add(habitacion);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Habitación creada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Ocurrió un error al guardar los datos: {ex.Message}";
                return View(habitacion);
            }
        }

        // GET: Habitaciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "No se encontró la habitación solicitada.";
                return RedirectToAction(nameof(Index));
            }

            var habitacion = await _context.Habitaciones.FindAsync(id);
            if (habitacion == null)
            {
                TempData["ErrorMessage"] = "La habitación no existe.";
                return RedirectToAction(nameof(Index));
            }

            return View(habitacion);
        }

        // POST: Habitaciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Habitacion habitacion)
        {
            if (id != habitacion.Id)
            {
                TempData["ErrorMessage"] = "Error de coincidencia en los datos.";
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Verifique los campos antes de guardar.";
                return View(habitacion);
            }

            try
            {
                _context.Update(habitacion);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Habitación actualizada correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Habitaciones.Any(e => e.Id == habitacion.Id))
                {
                    TempData["ErrorMessage"] = "La habitación ya no existe.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["ErrorMessage"] = "Ocurrió un error de concurrencia.";
                    return View(habitacion);
                }
            }
        }

        // GET: Habitaciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["ErrorMessage"] = "No se encontró la habitación a eliminar.";
                return RedirectToAction(nameof(Index));
            }

            var habitacion = await _context.Habitaciones
                .FirstOrDefaultAsync(m => m.Id == id);

            if (habitacion == null)
            {
                TempData["ErrorMessage"] = "La habitación no existe.";
                return RedirectToAction(nameof(Index));
            }

            return View(habitacion);
        }

        // POST: Habitaciones/DeleteConfirmed
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var habitacion = await _context.Habitaciones.FindAsync(id);
            if (habitacion != null)
            {
                _context.Habitaciones.Remove(habitacion);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Habitación eliminada correctamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "No se pudo eliminar la habitación.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool HabitacionExists(int id)
        {
            return _context.Habitaciones.Any(e => e.Id == id);
        }
    }
}
