using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClinicaVet.Data;
using ClinicaVet.Models;
using ClinicaVet.Services;
using ClinicaVet.ViewModels;

namespace ClinicaVet.Controllers
{
    [Authorize]
    public class TutoresController : Controller
    {
        private readonly ClinicaContext _context;
        private readonly ICpfCripto _cpfCripto;

        public TutoresController(ClinicaContext context, ICpfCripto cpfCripto)
        {
            _context = context;
            _cpfCripto = cpfCripto;
        }

        // GET: Tutores
        public async Task<IActionResult> Index(string? termo)
        {
            var consulta = _context.Tutores.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(termo))
            {
                consulta = consulta.Where(t =>
                    t.Nome.Contains(termo) || t.Email.Contains(termo));
            }

            ViewData["Termo"] = termo;
            return View(await consulta.OrderBy(t => t.Nome).ToListAsync());
        }

        // GET: Tutores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutor = await _context.Tutores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tutor == null)
            {
                return NotFound();
            }

            return View(tutor);
        }

        // GET: Tutores/Create
        public IActionResult Create()
        {
            return View(new TutorEditViewModel());
        }

        // POST: Tutores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TutorEditViewModel viewModel)
        {
            if (string.IsNullOrWhiteSpace(viewModel.CpfNovo))
            {
                ModelState.AddModelError(nameof(viewModel.CpfNovo), "Informe o CPF.");
            }

            if (ModelState.IsValid)
            {
                var tutor = new Tutor
                {
                    Nome = viewModel.Nome,
                    Email = viewModel.Email,
                    Telefone = viewModel.Telefone,
                    CpfCriptografado = _cpfCripto.Proteger(viewModel.CpfNovo!)
                };

                _context.Add(tutor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: Tutores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutor = await _context.Tutores.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
            if (tutor == null)
            {
                return NotFound();
            }

            var viewModel = new TutorEditViewModel
            {
                Id = tutor.Id,
                Nome = tutor.Nome,
                Email = tutor.Email,
                Telefone = tutor.Telefone
            };

            return View(viewModel);
        }

        // POST: Tutores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TutorEditViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var tutor = await _context.Tutores.FindAsync(id);
                if (tutor == null)
                {
                    return NotFound();
                }

                tutor.Nome = viewModel.Nome;
                tutor.Email = viewModel.Email;
                tutor.Telefone = viewModel.Telefone;

                if (!string.IsNullOrWhiteSpace(viewModel.CpfNovo))
                {
                    tutor.CpfCriptografado = _cpfCripto.Proteger(viewModel.CpfNovo);
                }

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TutorExists(viewModel.Id))
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
            return View(viewModel);
        }

        // GET: Tutores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutor = await _context.Tutores
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tutor == null)
            {
                return NotFound();
            }

            return View(tutor);
        }

        // POST: Tutores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tutor = await _context.Tutores.FindAsync(id);
            if (tutor != null)
            {
                _context.Tutores.Remove(tutor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TutorExists(int id)
        {
            return _context.Tutores.Any(e => e.Id == id);
        }
    }
}
