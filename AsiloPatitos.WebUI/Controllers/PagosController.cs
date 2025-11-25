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
    public class PagosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PagosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Pagos
        public async Task<IActionResult> Index()
        {
            var pagos = await _context.Pagos
                .Include(p => p.Reserva)
                .ToListAsync();

            return View(pagos);
        }

        // GET: Pagos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pago = await _context.Pagos
                .Include(p => p.Reserva)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pago == null)
            {
                return NotFound();
            }

            return View(pago);
        }

        // GET: Pagos/Create
        public IActionResult Create()
        {
            ViewData["ReservaId"] = new SelectList(_context.Reservas, "Id", "Id");
            return View();
        }

        // POST: Pagos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pago pago)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Por favor complete todos los campos requeridos.";
                ViewData["ReservaId"] = new SelectList(_context.Reservas, "Id", "Id", pago.ReservaId);
                return View(pago);
            }

            try
            {
                _context.Add(pago);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Pago registrado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ocurrió un error al guardar los datos: " + ex.Message;
                ViewData["ReservaId"] = new SelectList(_context.Reservas, "Id", "Id", pago.ReservaId);
                return View(pago);
            }
        }

        // GET: Pagos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var pago = await _context.Pagos.FindAsync(id);
            if (pago == null) return NotFound();

            ViewData["ReservaId"] = new SelectList(_context.Reservas, "Id", "Id", pago.ReservaId);
            return View(pago);
        }

        // POST: Pagos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Pago pago)
        {
            if (id != pago.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Por favor complete todos los campos requeridos.";
                ViewData["ReservaId"] = new SelectList(_context.Reservas, "Id", "Id", pago.ReservaId);
                return View(pago);
            }

            try
            {
                _context.Update(pago);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Pago actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PagoExists(pago.Id))
                    return NotFound();

                throw;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al actualizar el pago: " + ex.Message;
                ViewData["ReservaId"] = new SelectList(_context.Reservas, "Id", "Id", pago.ReservaId);
                return View(pago);
            }
        }

        // GET: Pagos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var pago = await _context.Pagos
                .Include(p => p.Reserva)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pago == null) return NotFound();

            return View(pago);
        }

        // POST: Pagos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pago = await _context.Pagos.FindAsync(id);

            if (pago == null)
            {
                TempData["ErrorMessage"] = "El registro no existe o ya fue eliminado.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Pagos.Remove(pago);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Pago eliminado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al eliminar: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        private bool PagoExists(int id)
        {
            return _context.Pagos.Any(e => e.Id == id);
        }
    }
}
