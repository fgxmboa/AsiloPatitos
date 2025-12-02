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
    public class TratamientosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TratamientosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Tratamientos
        public async Task<IActionResult> Index()
        {
            return View(await _context.Tratamientos.ToListAsync());
        }

        // GET: Tratamientos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tratamiento = await _context.Tratamientos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tratamiento == null)
            {
                return NotFound();
            }

            return View(tratamiento);
        }

        // GET: Tratamientos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tratamientos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Tratamiento tr)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Por favor complete todos los campos requeridos.";
                return View(tr);
            }

            try
            {
                _context.Add(tr);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Tratamiento creado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al guardar los datos: " + ex.Message;
                return View(tr);
            }
        }

        // GET: Tratamientos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var t = await _context.Tratamientos.FindAsync(id);
            if (t == null) return NotFound();

            return View(t);
        }

        // POST: Tratamientos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Tratamiento tr)
        {
            if (id != tr.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Por favor complete todos los campos requeridos.";
                return View(tr);
            }

            try
            {
                _context.Update(tr);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Tratamiento actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al actualizar los datos: " + ex.Message;
                return View(tr);
            }
        }

        // GET: Tratamientos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var t = await _context.Tratamientos.FirstOrDefaultAsync(x => x.Id == id);
            if (t == null) return NotFound();

            return View(t);
        }

        // POST: Tratamientos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var t = await _context.Tratamientos.FindAsync(id);
            if (t == null)
            {
                TempData["ErrorMessage"] = "No se encontró el tratamiento.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Tratamientos.Remove(t);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Tratamiento eliminado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al eliminar el tratamiento: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        private bool TratamientoExists(int id)
        {
            return _context.Tratamientos.Any(e => e.Id == id);
        }
    }
}
