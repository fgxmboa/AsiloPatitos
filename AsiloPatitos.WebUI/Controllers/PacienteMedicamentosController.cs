using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AsiloPatitos.Domain.Entities;
using AsiloPatitos.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AsiloPatitos.WebUI.Controllers
{
    [Authorize(Roles = "Administrador,Gerencia,Gestión de Pacientes")]
    public class PacienteMedicamentosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PacienteMedicamentosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PacienteMedicamentos
        public async Task<IActionResult> Index()
        {
            var registros = await _context.PacienteMedicamentos
                .Include(pm => pm.Paciente)
                .Include(pm => pm.Medicamento)
                .ToListAsync();
            return View(registros);
        }

        // GET: PacienteMedicamentos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var relacion = await _context.PacienteMedicamentos
                .Include(pm => pm.Paciente)
                .Include(pm => pm.Medicamento)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (relacion == null) return NotFound();

            return View(relacion);
        }

        // GET: PacienteMedicamentos/Create
        public IActionResult Create()
        {
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nombre");
            ViewData["MedicamentoId"] = new SelectList(_context.Medicamentos, "Id", "Nombre");
            return View();
        }

        // POST: PacienteMedicamentos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PacienteMedicamento pm)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Por favor complete todos los campos requeridos.";
                ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nombre", pm.PacienteId);
                ViewData["MedicamentoId"] = new SelectList(_context.Medicamentos, "Id", "Nombre", pm.MedicamentoId);
                return View(pm);
            }

            try
            {
                // Evitar duplicados
                bool existe = await _context.PacienteMedicamentos
                    .AnyAsync(x => x.PacienteId == pm.PacienteId && x.MedicamentoId == pm.MedicamentoId);

                if (existe)
                {
                    TempData["ErrorMessage"] = "Este paciente ya tiene asignado este medicamento.";
                    return RedirectToAction(nameof(Index));
                }

                _context.Add(pm);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Medicamento asignado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ocurrió un error al guardar los datos: " + ex.Message;
                return View(pm);
            }
        }

        // GET: PacienteMedicamentos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var pm = await _context.PacienteMedicamentos.FindAsync(id);
            if (pm == null) return NotFound();

            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nombre", pm.PacienteId);
            ViewData["MedicamentoId"] = new SelectList(_context.Medicamentos, "Id", "Nombre", pm.MedicamentoId);
            return View(pm);
        }

        // POST: PacienteMedicamentos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PacienteMedicamento pm)
        {
            if (id != pm.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Por favor revise los datos ingresados.";
                ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nombre", pm.PacienteId);
                ViewData["MedicamentoId"] = new SelectList(_context.Medicamentos, "Id", "Nombre", pm.MedicamentoId);
                return View(pm);
            }

            try
            {
                _context.Update(pm);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Asignación actualizada correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.PacienteMedicamentos.Any(e => e.Id == pm.Id))
                    return NotFound();

                throw;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al actualizar la información: " + ex.Message;
                return View(pm);
            }
        }

        // GET: PacienteMedicamentos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var pm = await _context.PacienteMedicamentos
                .Include(p => p.Paciente)
                .Include(p => p.Medicamento)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pm == null) return NotFound();

            return View(pm);
        }

        // POST: PacienteMedicamentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pm = await _context.PacienteMedicamentos.FindAsync(id);
            if (pm == null)
            {
                TempData["ErrorMessage"] = "Registro no encontrado.";
                return RedirectToAction(nameof(Index));
            }

            _context.PacienteMedicamentos.Remove(pm);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Asignación eliminada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        private bool PacienteMedicamentoExists(int id)
        {
            return _context.PacienteMedicamentos.Any(e => e.Id == id);
        }
    }
}
