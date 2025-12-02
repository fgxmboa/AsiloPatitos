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
    [Authorize(Roles = "Administrador,Gerencia,Recepción")]
    public class ReservasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReservasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reservas
        public async Task<IActionResult> Index()
        {
            var reservas = await _context.Reservas
                .Include(r => r.Paciente)
                .Include(r => r.Habitacion)
                .ToListAsync();

            return View(reservas);
        }

        // GET: Reservas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var reserva = await _context.Reservas
                .Include(r => r.Paciente)
                .Include(r => r.Habitacion)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (reserva == null) return NotFound();

            return View(reserva);
        }

        // GET: Reservas/Create
        public IActionResult Create()
        {
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nombre");
            ViewData["HabitacionId"] = new SelectList(_context.Habitaciones, "Id", "Numero");
            return View();
        }


        // POST: Reservas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reserva reserva)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Por favor complete todos los campos requeridos.";

                ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nombre", reserva.PacienteId);
                ViewData["HabitacionId"] = new SelectList(_context.Habitaciones, "Id", "Numero", reserva.HabitacionId);

                return View(reserva);
            }

            // ============================
            // VALIDACIÓN 1: EL PACIENTE NO PUEDE TENER DOS RESERVAS EN EL MISMO RANGO
            // ============================
            bool pacienteTieneChoque = await _context.Reservas.AnyAsync(r =>
                r.PacienteId == reserva.PacienteId &&
                r.Id != reserva.Id &&
                r.FechaIngreso < reserva.FechaSalida &&
                r.FechaSalida > reserva.FechaIngreso);

            if (pacienteTieneChoque)
            {
                TempData["ErrorMessage"] = "Este paciente ya tiene una reserva para estas fechas.";
                ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nombre", reserva.PacienteId);
                ViewData["HabitacionId"] = new SelectList(_context.Habitaciones, "Id", "Numero", reserva.HabitacionId);
                return View(reserva);
            }

            // ============================
            // VALIDACIÓN 2: LA HABITACIÓN NO PUEDE ESTAR RESERVADA EN EL MISMO RANGO
            // ============================
            bool habitacionOcupada = await _context.Reservas.AnyAsync(r =>
                r.HabitacionId == reserva.HabitacionId &&
                r.Id != reserva.Id &&
                r.FechaIngreso < reserva.FechaSalida &&
                r.FechaSalida > reserva.FechaIngreso);

            if (habitacionOcupada)
            {
                TempData["ErrorMessage"] = "La habitación seleccionada ya está reservada en este rango de fechas.";
                ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nombre", reserva.PacienteId);
                ViewData["HabitacionId"] = new SelectList(_context.Habitaciones, "Id", "Numero", reserva.HabitacionId);
                return View(reserva);
            }

            // Guardar si todo está bien
            try
            {
                _context.Add(reserva);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Reserva creada correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Ocurrió un error al guardar la reserva: " + ex.Message;

                ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nombre", reserva.PacienteId);
                ViewData["HabitacionId"] = new SelectList(_context.Habitaciones, "Id", "Numero", reserva.HabitacionId);

                return View(reserva);
            }
        }


        // GET: Reservas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null) return NotFound();

            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nombre", reserva.PacienteId);
            ViewData["HabitacionId"] = new SelectList(_context.Habitaciones, "Id", "Numero", reserva.HabitacionId);

            return View(reserva);
        }

        // POST: Reservas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Reserva reserva)
        {
            if (id != reserva.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Por favor complete todos los campos requeridos.";

                ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nombre", reserva.PacienteId);
                ViewData["HabitacionId"] = new SelectList(_context.Habitaciones, "Id", "Numero", reserva.HabitacionId);

                return View(reserva);
            }

            // VALIDACIÓN 1: Paciente no puede tener reservas cruzadas
            bool pacienteTieneChoque = await _context.Reservas.AnyAsync(r =>
                r.PacienteId == reserva.PacienteId &&
                r.Id != reserva.Id &&
                r.FechaIngreso < reserva.FechaSalida &&
                r.FechaSalida > reserva.FechaIngreso);

            if (pacienteTieneChoque)
            {
                TempData["ErrorMessage"] = "Este paciente ya tiene una reserva en este rango de fechas.";

                ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nombre", reserva.PacienteId);
                ViewData["HabitacionId"] = new SelectList(_context.Habitaciones, "Id", "Numero", reserva.HabitacionId);

                return View(reserva);
            }

            // VALIDACIÓN 2: Habitación no puede tener reservas cruzadas
            bool habitacionOcupada = await _context.Reservas.AnyAsync(r =>
                r.HabitacionId == reserva.HabitacionId &&
                r.Id != reserva.Id &&
                r.FechaIngreso < reserva.FechaSalida &&
                r.FechaSalida > reserva.FechaIngreso);

            if (habitacionOcupada)
            {
                TempData["ErrorMessage"] = "La habitación seleccionada ya está ocupada en este rango.";

                ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Nombre", reserva.PacienteId);
                ViewData["HabitacionId"] = new SelectList(_context.Habitaciones, "Id", "Numero", reserva.HabitacionId);

                return View(reserva);
            }

            try
            {
                _context.Update(reserva);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Reserva actualizada correctamente.";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["ErrorMessage"] = "Error al actualizar la reserva.";
                return View(reserva);
            }
        }



        // GET: Reservas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var reserva = await _context.Reservas
                .Include(r => r.Paciente)
                .Include(r => r.Habitacion)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (reserva == null) return NotFound();

            return View(reserva);
        }

        // POST: Reservas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva != null)
            {
                _context.Reservas.Remove(reserva);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Reserva eliminada correctamente.";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ReservaExists(int id)
        {
            return _context.Reservas.Any(e => e.Id == id);
        }
    }
}
