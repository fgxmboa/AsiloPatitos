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
            var applicationDbContext = _context.PacienteTratamientos.Include(p => p.Paciente).Include(p => p.Tratamiento);
            return View(await applicationDbContext.ToListAsync());
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
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Cedula");
            ViewData["TratamientoId"] = new SelectList(_context.Tratamientos, "Id", "Descripcion");
            return View();
        }

        // POST: PacienteTratamientos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PacienteId,TratamientoId,FechaAplicacion,Observaciones")] PacienteTratamiento pacienteTratamiento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pacienteTratamiento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Cedula", pacienteTratamiento.PacienteId);
            ViewData["TratamientoId"] = new SelectList(_context.Tratamientos, "Id", "Descripcion", pacienteTratamiento.TratamientoId);
            return View(pacienteTratamiento);
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
        public async Task<IActionResult> Delete(int? id)
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

        // POST: PacienteTratamientos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pacienteTratamiento = await _context.PacienteTratamientos.FindAsync(id);
            if (pacienteTratamiento != null)
            {
                _context.PacienteTratamientos.Remove(pacienteTratamiento);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PacienteTratamientoExists(int id)
        {
            return _context.PacienteTratamientos.Any(e => e.Id == id);
        }
    }
}
