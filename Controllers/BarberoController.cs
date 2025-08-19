using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiBarberiaApp.Data;
using MiBarberiaApp.Models;

namespace MiBarberiaApp.Controllers
{
    public class BarberoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BarberoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Barbero
        public async Task<IActionResult> Index()
        {
            return View(await _context.Barberos.ToListAsync());
        }

        // GET: Barbero/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var barbero = await _context.Barberos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (barbero == null)
            {
                return NotFound();
            }

            return View(barbero);
        }

        // GET: Barbero/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Barbero/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Activo")] Barbero barbero)
        {
            if (ModelState.IsValid)
            {
                _context.Add(barbero);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(barbero);
        }

        // GET: Barbero/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var barbero = await _context.Barberos.FindAsync(id);
            if (barbero == null)
            {
                return NotFound();
            }
            return View(barbero);
        }

        // POST: Barbero/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Activo")] Barbero barbero)
        {
            if (id != barbero.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(barbero);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BarberoExists(barbero.Id))
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
            return View(barbero);
        }

        // GET: Barbero/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var barbero = await _context.Barberos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (barbero == null)
            {
                return NotFound();
            }

            return View(barbero);
        }

        // POST: Barbero/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var barbero = await _context.Barberos.FindAsync(id);
            if (barbero != null)
            {
                _context.Barberos.Remove(barbero);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BarberoExists(int id)
        {
            return _context.Barberos.Any(e => e.Id == id);
        }
    }
}
