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
    public class RolesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RolesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Roles
        public async Task<IActionResult> Index()
        {
            var roles = await _context.Roles.ToListAsync();
            return View(roles);
        }

        // GET: Roles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rol = await _context.Roles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rol == null)
            {
                return NotFound();
            }

            return View(rol);
        }

        // GET: Roles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Roles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Rol rol)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Por favor, complete los campos correctamente.";
                return View(rol);
            }

            try
            {
                bool existe = await _context.Roles.AnyAsync(r => r.Nombre == rol.Nombre);
                if (existe)
                {
                    TempData["ErrorMessage"] = "Ya existe un rol con ese nombre.";
                    return View(rol);
                }

                _context.Add(rol);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Rol creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al crear el rol: " + ex.Message;
                return View(rol);
            }
        }

        // GET: Roles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var rol = await _context.Roles.FindAsync(id);
            if (rol == null)
                return NotFound();

            return View(rol);
        }

        // POST: Roles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Rol rol)
        {
            if (id != rol.Id)
                return NotFound();

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Revise los campos antes de guardar.";
                return View(rol);
            }

            try
            {
                bool existeDuplicado = await _context.Roles
                    .AnyAsync(r => r.Nombre == rol.Nombre && r.Id != rol.Id);

                if (existeDuplicado)
                {
                    TempData["ErrorMessage"] = "Ya existe otro rol con ese nombre.";
                    return View(rol);
                }

                _context.Update(rol);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Rol actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Roles.Any(e => e.Id == rol.Id))
                    return NotFound();

                throw;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al actualizar: " + ex.Message;
                return View(rol);
            }
        }

        // GET: Roles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var rol = await _context.Roles.FirstOrDefaultAsync(m => m.Id == id);
            if (rol == null)
                return NotFound();

            return View(rol);
        }


        // POST: Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rol = await _context.Roles.FindAsync(id);
            if (rol == null)
            {
                TempData["ErrorMessage"] = "El rol ya fue eliminado o no existe.";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Roles.Remove(rol);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Rol eliminado correctamente.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error al eliminar el rol: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        private bool RolExists(int id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }
    }
}
