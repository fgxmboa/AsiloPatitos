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
    [Authorize(Roles = "Administrador,Gerencia")]
    public class EmpleadosController : Controller
    {
        private readonly ILogger<EmpleadosController> _logger;
        private readonly ApplicationDbContext _context;

        public EmpleadosController(ApplicationDbContext context, ILogger<EmpleadosController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Empleados
        public async Task<IActionResult> Index()
        {
            return View(await _context.Empleados.ToListAsync());
        }

        // GET: Empleados/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // GET: Empleados/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Empleados/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Empleado empleado)
        {
            if (!ModelState.IsValid) return View(empleado);

            try
            {
                bool existe = await _context.Empleados
                    .AnyAsync(e => e.Cedula == empleado.Cedula);
                if (existe)
                {
                    ModelState.AddModelError("Cedula", "Ya existe un empleado con esta cédula.");
                    return View(empleado);
                }

                _context.Add(empleado);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Empleado creado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error creando empleado");
                TempData["Error"] = "No se pudo guardar el empleado. Intenta de nuevo.";
                return View(empleado);
            }
        }

        // GET: Empleados/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }
            return View(empleado);
        }

        // POST: Empleados/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Empleado empleado)
        {
            if (id != empleado.Id) return NotFound();
            if (!ModelState.IsValid) return View(empleado);

            try
            {
                bool cedulaTomada = await _context.Empleados
                    .AnyAsync(e => e.Cedula == empleado.Cedula && e.Id != empleado.Id);
                if (cedulaTomada)
                {
                    ModelState.AddModelError("Cedula", "La cédula ya está asignada a otro empleado.");
                    return View(empleado);
                }

                _context.Update(empleado);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Empleado actualizado.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!await _context.Empleados.AnyAsync(e => e.Id == empleado.Id))
                    return NotFound();

                _logger.LogError(ex, "Concurrencia editando empleado");
                TempData["Error"] = "Otro usuario modificó este registro. Refresca la página.";
                return View(empleado);
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error editando empleado");
                TempData["Error"] = "No se pudo actualizar el empleado.";
                return View(empleado);
            }
        }


        // GET: Empleados/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // POST: Empleados/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var empleado = await _context.Empleados.FindAsync(id);
                if (empleado == null) return NotFound();

                _context.Empleados.Remove(empleado);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Empleado eliminado.";
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error eliminando empleado {Id}", id);
                TempData["Error"] = "No se puede eliminar el empleado. Puede tener dependencias.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
