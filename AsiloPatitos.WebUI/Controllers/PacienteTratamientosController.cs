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
    public class PacienteTratamientosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PacienteTratamientosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PacienteTratamientos
        public async Task<IActionResult> Index()
        {
            var datos = await _context.PacienteTratamientos
                .Include(p => p.Paciente)
                .Include(t => t.Tratamiento)
                .ToListAsync();

            return View(datos);
        }

        // GET: PacienteTratamientos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pacienteTratamiento = await _context.PacienteTratamientos
                .Include(p => p.Paciente)
                .Include(p => p.Tratamiento)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pacienteTratamiento == null)
            {
                return NotFound();
            }

            return View(pacienteTratamiento);
        }

        // GET: PacienteTratamientos/Create
        public IActionResult Create()
        {
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nombre");
            ViewData["TratamientoId"] = new SelectList(_context.Tratamientos, "Id", "Nombre");
            return View();
        }

        // POST: PacienteTratamientos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PacienteTratamiento pt)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Por favor complete todos los campos requeridos.";
                ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nombre", pt.PacienteId);
                ViewData["TratamientoId"] = new SelectList(_context.Tratamientos, "Id", "Nombre", pt.TratamientoId);
                return View(pt);
            }

            try
            {
                // Evitar duplicados del mismo tratamiento en la misma fecha
                bool existe = await _context.PacienteTratamientos
                    .AnyAsync(x =>
                        x.PacienteId == pt.PacienteId &&
                        x.TratamientoId == pt.TratamientoId &&
                        x.FechaAplicacion.Date == pt.FechaAplicacion.Date);

                if (existe)
                {
                    TempData["ErrorMessage"] = "Este tratamiento ya fue aplicado al paciente en esa fecha.";
                    return RedirectToAction(nameof(Index));
                }

                _context.Add(pt);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Tratamiento registrado correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ocurrió un error al guardar los datos: " + ex.Message;
                return View(pt);
            }
        }

        // GET: PacienteTratamientos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pacienteTratamiento = await _context.PacienteTratamientos.FindAsync(id);
            if (pacienteTratamiento == null)
            {
                return NotFound();
            }
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Cedula", pacienteTratamiento.PacienteId);
            ViewData["TratamientoId"] = new SelectList(_context.Tratamientos, "Id", "Descripcion", pacienteTratamiento.TratamientoId);
            return View(pacienteTratamiento);
        }

        // POST: PacienteTratamientos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PacienteId,TratamientoId,FechaAplicacion,Observaciones")] PacienteTratamiento pacienteTratamiento)
        {
            if (id != pacienteTratamiento.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pacienteTratamiento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PacienteTratamientoExists(pacienteTratamiento.Id))
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
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Cedula", pacienteTratamiento.PacienteId);
            ViewData["TratamientoId"] = new SelectList(_context.Tratamientos, "Id", "Descripcion", pacienteTratamiento.TratamientoId);
            return View(pacienteTratamiento);
        }

        // GET: PacienteTratamientos/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.PacienteTratamientos
                .Include(p => p.Paciente)
                .Include(t => t.Tratamiento)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (item == null) return NotFound();

            return View(item);
        }

        // POST: PacienteTratamientos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.PacienteTratamientos.FindAsync(id);

            if (item != null)
            {
                _context.PacienteTratamientos.Remove(item);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Registro eliminado correctamente.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PacienteTratamientoExists(int id)
        {
            return _context.PacienteTratamientos.Any(e => e.Id == id);
        }
    }
}
