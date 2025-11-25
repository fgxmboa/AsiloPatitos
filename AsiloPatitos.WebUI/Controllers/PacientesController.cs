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
    public class PacientesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PacientesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Pacientes
        public async Task<IActionResult> Index()
        {
            var pacientes = await _context.Pacientes.Include(p => p.Habitacion).ToListAsync();
            return View(pacientes);
        }

        // GET: Pacientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var paciente = await _context.Pacientes
                .Include(p => p.Habitacion)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (paciente == null)
                return NotFound();

            return View(paciente);
        }

        // GET: Pacientes/Create
        public IActionResult Create()
        {
            TempData.Remove("SuccessMessage");
            TempData.Remove("ErrorMessage");
            ViewData["HabitacionId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Habitaciones, "Id", "Numero");
            return View();
        }

        // POST: Pacientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Paciente paciente)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Por favor, complete todos los campos requeridos correctamente.";
                ViewData["HabitacionId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Habitaciones, "Id", "Numero", paciente.HabitacionId);
                return View(paciente);
            }

            try
            {
                bool existe = await _context.Pacientes.AnyAsync(p => p.Cedula == paciente.Cedula);
                if (existe)
                {
                    TempData["ErrorMessage"] = "Ya existe un paciente registrado con esta cédula.";
                    return RedirectToAction(nameof(Index));
                }

                _context.Add(paciente);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Paciente agregado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ocurrió un error al guardar los datos: " + ex.Message;
                return View(paciente);
            }
        }

        // GET: Pacientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null)
                return NotFound();

            ViewData["HabitacionId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Habitaciones, "Id", "Numero", paciente.HabitacionId);
            return View(paciente);
        }

        // POST: Pacientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Paciente paciente)
        {
            if (id != paciente.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Por favor, revise los datos ingresados.";
                return View(paciente);
            }

            try
            {
                _context.Update(paciente);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Datos del paciente actualizados correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Pacientes.Any(p => p.Id == paciente.Id))
                    return NotFound();
                else
                    throw;
            }
        }

        // GET: Pacientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var paciente = await _context.Pacientes
                .Include(p => p.Habitacion)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (paciente == null)
                return NotFound();

            return View(paciente);
        }

        // POST: Pacientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente != null)
            {
                _context.Pacientes.Remove(paciente);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Paciente eliminado correctamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "No se encontró el paciente a eliminar.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PacienteExists(int id)
        {
            return _context.Pacientes.Any(e => e.Id == id);
        }
    }
}
