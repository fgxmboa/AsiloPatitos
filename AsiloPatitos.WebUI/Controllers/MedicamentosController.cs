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
    public class MedicamentosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MedicamentosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Medicamentos
        public async Task<IActionResult> Index()
        {
            var medicamentos = await _context.Medicamentos.ToListAsync();
            return View(medicamentos);
        }

        // GET: Medicamentos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var medicamento = await _context.Medicamentos.FirstOrDefaultAsync(m => m.Id == id);
            if (medicamento == null)
                return NotFound();

            return View(medicamento);
        }

        // GET: Medicamentos/Create
        public IActionResult Create()
        {
            TempData.Remove("SuccessMessage");
            TempData.Remove("ErrorMessage");
            return View();
        }

        // POST: Medicamentos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Medicamento medicamento)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Por favor, complete todos los campos requeridos correctamente.";
                return View(medicamento);
            }

            try
            {
                bool existe = await _context.Medicamentos.AnyAsync(m => m.Nombre == medicamento.Nombre);

                if (existe)
                {
                    TempData["ErrorMessage"] = "Ya existe un medicamento con este nombre.";
                    return View(medicamento);
                }

                _context.Add(medicamento);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Medicamento creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al guardar el medicamento: " + ex.Message;
                return View(medicamento);
            }
        }

        // GET: Medicamentos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var medicamento = await _context.Medicamentos.FindAsync(id);
            if (medicamento == null)
                return NotFound();

            return View(medicamento);
        }

        // POST: Medicamentos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Medicamento medicamento)
        {
            if (id != medicamento.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Por favor, revise los datos ingresados.";
                return View(medicamento);
            }

            try
            {
                _context.Update(medicamento);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Medicamento actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Medicamentos.Any(m => m.Id == medicamento.Id))
                    return NotFound();
                else
                    throw;
            }
        }

        // GET: Medicamentos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var medicamento = await _context.Medicamentos.FirstOrDefaultAsync(m => m.Id == id);
            if (medicamento == null)
                return NotFound();

            return View(medicamento);
        }

        // POST: Medicamentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicamento = await _context.Medicamentos.FindAsync(id);
            if (medicamento != null)
            {
                _context.Medicamentos.Remove(medicamento);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Medicamento eliminado correctamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "No se encontró el medicamento a eliminar.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool MedicamentoExists(int id)
        {
            return _context.Medicamentos.Any(e => e.Id == id);
        }
    }
}
