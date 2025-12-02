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
    public class EmpleadoRolesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmpleadoRolesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: EmpleadoRoles
        public async Task<IActionResult> Index()
        {
            var roles = _context.EmpleadoRoles
                .Include(e => e.Empleado)
                .Include(e => e.Rol);
            return View(await roles.ToListAsync());
        }

        // GET: EmpleadoRoles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleadoRol = await _context.EmpleadoRoles
                .Include(e => e.Empleado)
                .Include(e => e.Rol)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empleadoRol == null)
            {
                return NotFound();
            }

            return View(empleadoRol);
        }

        // GET: EmpleadoRoles/Create
        public IActionResult Create()
        {
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Nombre");
            ViewData["RolId"] = new SelectList(_context.Roles, "Id", "Nombre");
            return View();
        }

        // POST: EmpleadoRoles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmpleadoRol empleadoRol)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Por favor, complete todos los campos requeridos.";
                ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Nombre", empleadoRol.EmpleadoId);
                ViewData["RolId"] = new SelectList(_context.Roles, "Id", "Nombre", empleadoRol.RolId);
                return View(empleadoRol);
            }

            try
            {
                // Validación: evitar duplicados
                bool existe = await _context.EmpleadoRoles
                    .AnyAsync(er => er.EmpleadoId == empleadoRol.EmpleadoId && er.RolId == empleadoRol.RolId);

                if (existe)
                {
                    TempData["ErrorMessage"] = "Este empleado ya tiene asignado este rol.";
                    return RedirectToAction(nameof(Index));
                }

                _context.Add(empleadoRol);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Rol asignado exitosamente al empleado.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ocurrió un error al guardar los datos: " + ex.Message;
                return View(empleadoRol);
            }
        }

        // GET: EmpleadoRoles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleadoRol = await _context.EmpleadoRoles.FindAsync(id);
            if (empleadoRol == null)
            {
                return NotFound();
            }
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Cedula", empleadoRol.EmpleadoId);
            ViewData["RolId"] = new SelectList(_context.Roles, "Id", "Nombre", empleadoRol.RolId);
            return View(empleadoRol);
        }

        // POST: EmpleadoRoles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmpleadoId,RolId")] EmpleadoRol empleadoRol)
        {
            if (id != empleadoRol.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(empleadoRol);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpleadoRolExists(empleadoRol.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Cedula", empleadoRol.EmpleadoId);
            ViewData["RolId"] = new SelectList(_context.Roles, "Id", "Nombre", empleadoRol.RolId);
            return View(empleadoRol);
        }

        // GET: EmpleadoRoles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var empleadoRol = await _context.EmpleadoRoles
                .Include(e => e.Empleado)
                .Include(e => e.Rol)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (empleadoRol == null)
                return NotFound();

            return View(empleadoRol);
        }

        // POST: EmpleadoRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var empleadoRol = await _context.EmpleadoRoles.FindAsync(id);
            if (empleadoRol == null)
            {
                TempData["ErrorMessage"] = "El registro no existe o ya fue eliminado.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.EmpleadoRoles.Remove(empleadoRol);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Rol eliminado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al eliminar el registro: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        private bool EmpleadoRolExists(int id)
        {
            return _context.EmpleadoRoles.Any(e => e.Id == id);
        }
    }
}
