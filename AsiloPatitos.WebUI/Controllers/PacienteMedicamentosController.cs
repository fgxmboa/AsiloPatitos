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
    public class PacienteMedicamentosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PacienteMedicamentosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PacienteMedicamentos
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PacienteMedicamentos.Include(p => p.Medicamento).Include(p => p.Paciente);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PacienteMedicamentos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pacienteMedicamento = await _context.PacienteMedicamentos
                .Include(p => p.Medicamento)
                .Include(p => p.Paciente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pacienteMedicamento == null)
            {
                return NotFound();
            }

            return View(pacienteMedicamento);
        }

        // GET: PacienteMedicamentos/Create
        public IActionResult Create()
        {
            ViewData["MedicamentoId"] = new SelectList(_context.Medicamentos, "Id", "Dosis");
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Cedula");
            return View();
        }

        // POST: PacienteMedicamentos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PacienteId,MedicamentoId,FechaInicio,FechaFin,Indicaciones")] PacienteMedicamento pacienteMedicamento)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pacienteMedicamento);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MedicamentoId"] = new SelectList(_context.Medicamentos, "Id", "Dosis", pacienteMedicamento.MedicamentoId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Cedula", pacienteMedicamento.PacienteId);
            return View(pacienteMedicamento);
        }

        // GET: PacienteMedicamentos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pacienteMedicamento = await _context.PacienteMedicamentos.FindAsync(id);
            if (pacienteMedicamento == null)
            {
                return NotFound();
            }
            ViewData["MedicamentoId"] = new SelectList(_context.Medicamentos, "Id", "Dosis", pacienteMedicamento.MedicamentoId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Cedula", pacienteMedicamento.PacienteId);
            return View(pacienteMedicamento);
        }

        // POST: PacienteMedicamentos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PacienteId,MedicamentoId,FechaInicio,FechaFin,Indicaciones")] PacienteMedicamento pacienteMedicamento)
        {
            if (id != pacienteMedicamento.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pacienteMedicamento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PacienteMedicamentoExists(pacienteMedicamento.Id))
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
            ViewData["MedicamentoId"] = new SelectList(_context.Medicamentos, "Id", "Dosis", pacienteMedicamento.MedicamentoId);
            ViewData["PacienteId"] = new SelectList(_context.Pacientes, "Id", "Cedula", pacienteMedicamento.PacienteId);
            return View(pacienteMedicamento);
        }

        // GET: PacienteMedicamentos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pacienteMedicamento = await _context.PacienteMedicamentos
                .Include(p => p.Medicamento)
                .Include(p => p.Paciente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pacienteMedicamento == null)
            {
                return NotFound();
            }

            return View(pacienteMedicamento);
        }

        // POST: PacienteMedicamentos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pacienteMedicamento = await _context.PacienteMedicamentos.FindAsync(id);
            if (pacienteMedicamento != null)
            {
                _context.PacienteMedicamentos.Remove(pacienteMedicamento);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PacienteMedicamentoExists(int id)
        {
            return _context.PacienteMedicamentos.Any(e => e.Id == id);
        }
    }
}
