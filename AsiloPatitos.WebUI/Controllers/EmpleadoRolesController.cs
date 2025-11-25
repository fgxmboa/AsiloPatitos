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
            var applicationDbContext = _context.EmpleadoRoles.Include(e => e.Empleado).Include(e => e.Rol);
            return View(await applicationDbContext.ToListAsync());
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
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Cedula");
            ViewData["RolId"] = new SelectList(_context.Roles, "Id", "Nombre");
            return View();
        }

        // POST: EmpleadoRoles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EmpleadoId,RolId")] EmpleadoRol empleadoRol)
        {
            if (ModelState.IsValid)
            {
                _context.Add(empleadoRol);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Cedula", empleadoRol.EmpleadoId);
            ViewData["RolId"] = new SelectList(_context.Roles, "Id", "Nombre", empleadoRol.RolId);
            return View(empleadoRol);
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

        // POST: EmpleadoRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var empleadoRol = await _context.EmpleadoRoles.FindAsync(id);
            if (empleadoRol != null)
            {
                _context.EmpleadoRoles.Remove(empleadoRol);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmpleadoRolExists(int id)
        {
            return _context.EmpleadoRoles.Any(e => e.Id == id);
        }
    }
}
