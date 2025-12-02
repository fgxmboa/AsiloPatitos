using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AsiloPatitos.Domain.Entities;
using AsiloPatitos.Infrastructure;
using AsiloPatitos.WebUI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace AsiloPatitos.WebUI.Controllers
{
    [Authorize(Roles = "Administrador,Gerencia")]
    public class ReportesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reportes
        public async Task<IActionResult> Index()
        {
            // 1. Cantidad de pacientes registrados
            int pacientesRegistrados = await _context.Pacientes.CountAsync();

            // 2. Cantidad de pacientes alojados (pacientes con reserva activa)
            int pacientesAlojados = await _context.Reservas
                .CountAsync(r => r.FechaIngreso <= DateTime.Today
                                 && r.FechaSalida >= DateTime.Today);

            // 3. Cantidad de pacientes alojados por día (HOY)
            int pacientesPorDia = pacientesAlojados;

            // 4. Habitaciones reservadas vs totales
            int habitacionesTotales = await _context.Habitaciones.CountAsync();
            int habitacionesOcupadas = await _context.Reservas
                .CountAsync(r => r.FechaIngreso <= DateTime.Today
                                 && r.FechaSalida >= DateTime.Today);

            var model = new ReporteViewModel
            {
                PacientesRegistrados = pacientesRegistrados,
                PacientesAlojados = pacientesAlojados,
                PacientesPorDia = pacientesPorDia,
                HabitacionesTotales = habitacionesTotales,
                HabitacionesOcupadas = habitacionesOcupadas
            };

            return View(model);
        }

        // GET: Reportes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reporte = await _context.Reportes
                .Include(r => r.Empleado)
                .Include(r => r.Paciente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reporte == null)
            {
                return NotFound();
            }

            return View(reporte);
        }

        // GET: Reportes/Create
        public IActionResult Create()
        {
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Cedula");
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Cedula");
            return View();
        }

        // POST: Reportes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EmpleadoId,PacienteId,Titulo,Descripcion,FechaCreacion,Tipo")] Reporte reporte)
        {
            if (ModelState.IsValid)
            {
                _context.Add(reporte);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Cedula", reporte.EmpleadoId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Cedula", reporte.PacienteId);
            return View(reporte);
        }

        // GET: Reportes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reporte = await _context.Reportes.FindAsync(id);
            if (reporte == null)
            {
                return NotFound();
            }
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Cedula", reporte.EmpleadoId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Cedula", reporte.PacienteId);
            return View(reporte);
        }

        // POST: Reportes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmpleadoId,PacienteId,Titulo,Descripcion,FechaCreacion,Tipo")] Reporte reporte)
        {
            if (id != reporte.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reporte);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReporteExists(reporte.Id))
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
            ViewData["EmpleadoId"] = new SelectList(_context.Empleados, "Id", "Cedula", reporte.EmpleadoId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Cedula", reporte.PacienteId);
            return View(reporte);
        }

        // GET: Reportes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reporte = await _context.Reportes
                .Include(r => r.Empleado)
                .Include(r => r.Paciente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reporte == null)
            {
                return NotFound();
            }

            return View(reporte);
        }

        // POST: Reportes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reporte = await _context.Reportes.FindAsync(id);
            if (reporte != null)
            {
                _context.Reportes.Remove(reporte);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReporteExists(int id)
        {
            return _context.Reportes.Any(e => e.Id == id);
        }
    }
}
